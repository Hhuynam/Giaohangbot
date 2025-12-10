using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;
using System.Text;

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

    // Hard-code config values
    public const string MongoConnectionString =
        "mongodb://hhn51023_db_user:Namha51023%4001@ac-11ljvhd-shard-00-00.o39loax.mongodb.net:27017," +
        "ac-11ljvhd-shard-00-01.o39loax.mongodb.net:27017," +
        "ac-11ljvhd-shard-00-02.o39loax.mongodb.net:27017/?ssl=true&replicaSet=atlas-luprks-shard-0&authSource=admin&retryWrites=true&w=majority";

    public const string MongoDatabaseName = "giaohangbot";
    public const string ProductCollectionName = "products_new";
    public const string AccountCollectionName = "accounts";

    public const string VietQR_AccountNo = "1234567890";
    public const string VietQR_AccountName = "CONG TY GIAO HANG BOT";
    public const int VietQR_AcqId = 970436;

    public const string MqttBrokerHost = "broker.hivemq.com";
    public const int MqttBrokerPort = 1883;
    public const string MqttClientId = "GiaohangbotApp";
    public const string MqttTopic = "namha/iot";


    public const string AdminStreamUrl = "http://192.168.0.220/stream";

    public readonly IMongoDatabase _database;
    public readonly GridFSBucket _imageBucket;
    public readonly IMongoCollection<Product> _productCollection;
    public readonly IMongoCollection<Account> _accountCollection;

    public AppService()
    {
        var client = new MongoClient(MongoConnectionString);
        _database = client.GetDatabase(MongoDatabaseName);
        _imageBucket = new GridFSBucket(_database);
        _productCollection = _database.GetCollection<Product>(ProductCollectionName);
        _accountCollection = _database.GetCollection<Account>(AccountCollectionName);
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

    public async Task<string> GetNextCargoIdAsync()
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
            accountNo = VietQR_AccountNo,
            accountName = VietQR_AccountName,
            acqId = VietQR_AcqId,
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
        var mqtt = new MqttService();
        await mqtt.ConnectAsync();
        string emvco = await GenerateVietQrAsync(product.Cargo_Id, product.Price);
        string payload = $"emvco={emvco}";
        await mqtt.PublishAsync(payload);
        await mqtt.DisconnectAsync();
    }

    public async Task PublishMqttCommandAsync(string command)
    {
        var mqtt = new MqttService();
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
    public readonly IMqttClient _client;
    public readonly MqttClientOptions _options;

    public MqttService(string clientId = null)
    {
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

        var cid = clientId ?? AppService.MqttClientId;

        _options = new MqttClientOptionsBuilder()
            .WithClientId(cid)
            .WithTcpServer(AppService.MqttBrokerHost, AppService.MqttBrokerPort)
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
            .WithTopic(AppService.MqttTopic)
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
