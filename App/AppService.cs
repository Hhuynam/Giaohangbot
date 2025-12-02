using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Configuration;

public class AppService
{
    public class Product
    {
        public ObjectId Id { get; set; }
        public string Cargo_Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public ObjectId ImageId { get; set; }
        public DateTime TimeUpload { get; set; }
        public bool IsPaid { get; set; }
    }

    public class Account
    {
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private readonly IMongoDatabase _database;
    private readonly GridFSBucket _imageBucket;
    private readonly IMongoCollection<Product> _productCollection;
    private readonly IMongoCollection<Account> _accountCollection;
    private readonly IConfiguration _config;

    public AppService(IConfiguration config)
    {
        _config = config;

        var connectionString = _config["MongoDB:ConnectionString"];
        var dbName = _config["MongoDB:DatabaseName"];
        var client = new MongoClient(connectionString);

        _database = client.GetDatabase(dbName);
        _imageBucket = new GridFSBucket(_database);
        _productCollection = _database.GetCollection<Product>(_config["MongoDB:ProductCollection"]);
        _accountCollection = _database.GetCollection<Account>(_config["MongoDB:AccountCollection"]);
    }

    // Factory async method để tạo AppService
    public static async Task<AppService> CreateAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("appsettings.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        var config = new ConfigurationBuilder()
            .AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(json)))
            .Build();

        return new AppService(config);
    }

    public async Task<ObjectId> SaveImageAsync(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return await _imageBucket.UploadFromStreamAsync(Path.GetFileName(filePath), stream);
    }

    public async Task<byte[]> GetImageAsync(ObjectId imageId)
    {
        using var ms = new MemoryStream();
        await _imageBucket.DownloadToStreamAsync(imageId, ms);
        return ms.ToArray();
    }

    private async Task<string> GetNextCargoIdAsync()
    {
        var counters = _database.GetCollection<BsonDocument>("counters");
        var filter = Builders<BsonDocument>.Filter.Eq("_id", "CargoCounter");
        var update = Builders<BsonDocument>.Update.Inc("seq", 1);
        var options = new FindOneAndUpdateOptions<BsonDocument>
        {
            IsUpsert = true,
            ReturnDocument = ReturnDocument.After
        };
        var result = await counters.FindOneAndUpdateAsync(filter, update, options);
        int seq = result["seq"].AsInt32;
        return $"DH_{seq:D3}";
    }

    public async Task SaveProductAsync(Product product)
    {
        product.Cargo_Id = await GetNextCargoIdAsync();
        product.TimeUpload = DateTime.UtcNow;
        product.IsPaid = false;
        await _productCollection.InsertOneAsync(product);
    }

    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productCollection.Find("{}").ToListAsync();
    }

    public async Task<Product> FindProductByCargoIdAsync(string cargoId)
    {
        var filter = Builders<Product>.Filter.Eq("Cargo_Id", cargoId);
        return await _productCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task UpdatePaymentStatusAsync(string cargoId, bool isPaid)
    {
        var filter = Builders<Product>.Filter.Eq("Cargo_Id", cargoId);
        var update = Builders<Product>.Update.Set(p => p.IsPaid, isPaid);
        await _productCollection.UpdateOneAsync(filter, update);
    }

    public async Task<string> GenerateVietQrAsync(string cargoId, double amount)
    {
        using var client = new HttpClient();
        var payload = new
        {
            accountNo = _config["VietQR:AccountNo"],
            accountName = _config["VietQR:AccountName"],
            acqId = int.Parse(_config["VietQR:AcqId"]),
            amount = amount,
            addInfo = cargoId,
            format = "text",
            template = "qr_only"
        };
        var json = JsonConvert.SerializeObject(payload);
        var response = await client.PostAsync(
            "https://api.vietqr.io/v2/generate",
            new StringContent(json, Encoding.UTF8, "application/json")
        );
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(result);
        return obj.data.qrCode;
    }

    public async Task PublishOrderToMqttAsync(Product product)
    {
        var mqtt = new MqttService(_config);
        await mqtt.ConnectAsync();
        string emvco = await GenerateVietQrAsync(product.Cargo_Id, product.Price);
        string payload = $"emvco={emvco}";
        await mqtt.PublishAsync(payload);
        await mqtt.DisconnectAsync();
    }

    public async Task PublishMqttCommandAsync(string command)
    {
        var mqtt = new MqttService(_config);
        await mqtt.ConnectAsync();
        await mqtt.PublishAsync(command);
        await mqtt.DisconnectAsync();
    }

    public async Task CreateAccountAsync(string username, string password, string role)
    {
        var account = new Account
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role,
            CreatedAt = DateTime.UtcNow
        };
        await _accountCollection.InsertOneAsync(account);
    }

    public async Task<Account> LoginAsync(string username, string password)
    {
        var filter = Builders<Account>.Filter.Eq(a => a.Username, username);
        var account = await _accountCollection.Find(filter).FirstOrDefaultAsync();
        if (account != null && BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
        {
            return account;
        }
        return null;
    }

    public bool IsAdmin(Account account)
    {
        return account.Role == "Admin";
    }
}

public class MqttService
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;
    private readonly IConfiguration _config;

    public MqttService(IConfiguration config, string clientId = null)
    {
        _config = config;
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

        var brokerHost = _config["MQTT:BrokerHost"];
        var brokerPort = int.Parse(_config["MQTT:BrokerPort"]);
        var cid = clientId ?? _config["MQTT:ClientId"];

        _options = new MqttClientOptionsBuilder()
            .WithClientId(cid)
            .WithTcpServer(brokerHost, brokerPort)
            .WithCleanSession()
            .Build();
    }

    public bool IsConnected => _client.IsConnected;

    public async Task ConnectAsync()
    {
        if (!_client.IsConnected)
            await _client.ConnectAsync(_options);
    }

    public async Task PublishAsync(string message)
    {
        if (!_client.IsConnected)
            await ConnectAsync();
        var mqttMessage = new MqttApplicationMessageBuilder()
            .WithTopic(_config["MQTT:Topic"])
            .WithPayload(Encoding.UTF8.GetBytes(message))
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
            .WithRetainFlag(false)
            .Build();
        await _client.PublishAsync(mqttMessage);
    }

    public async Task DisconnectAsync()
    {
        if (_client.IsConnected)
            await _client.DisconnectAsync();
    }
}
