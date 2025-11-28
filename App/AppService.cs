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


    private readonly IMongoDatabase _database;
    private readonly GridFSBucket _imageBucket;
    private readonly IMongoCollection<Product> _productCollection;

    public AppService()
    {
        var connectionString =
            "mongodb://hhn51023_db_user:Namha51023%4001@" +
            "ac-11ljvhd-shard-00-00.o39loax.mongodb.net:27017," +
            "ac-11ljvhd-shard-00-01.o39loax.mongodb.net:27017," +
            "ac-11ljvhd-shard-00-02.o39loax.mongodb.net:27017/" +
            "?ssl=true&replicaSet=atlas-luprks-shard-0&authSource=admin&retryWrites=true&w=majority";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase("giaohangbot");
        _imageBucket = new GridFSBucket(_database);
        _productCollection = _database.GetCollection<Product>("products_new");
    }

    // Save image to GridFS
    public async Task<ObjectId> SaveImageAsync(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        return await _imageBucket.UploadFromStreamAsync(Path.GetFileName(filePath), stream);
    }

    // Get image from GridFS
    public async Task<byte[]> GetImageAsync(ObjectId imageId)
    {
        using var ms = new MemoryStream();
        await _imageBucket.DownloadToStreamAsync(imageId, ms);
        return ms.ToArray();
    }

    // Generate next Cargo_Id
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

    // Save product
    public async Task SaveProductAsync(Product product)
    {
        product.Cargo_Id = await GetNextCargoIdAsync();
        product.TimeUpload = DateTime.UtcNow;
        product.IsPaid = false;
        await _productCollection.InsertOneAsync(product);
    }

    // Get all products
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _productCollection.Find("{}").ToListAsync();
    }

    // Find product by Cargo_Id
    public async Task<Product> FindProductByCargoIdAsync(string cargoId)
    {
        var filter = Builders<Product>.Filter.Eq("Cargo_Id", cargoId);
        return await _productCollection.Find(filter).FirstOrDefaultAsync();
    }

    // Update payment status
    public async Task UpdatePaymentStatusAsync(string cargoId, bool isPaid)
    {
        var filter = Builders<Product>.Filter.Eq("Cargo_Id", cargoId);
        var update = Builders<Product>.Update.Set(p => p.IsPaid, isPaid);
        await _productCollection.UpdateOneAsync(filter, update);
    }

    // Hàm chỉ gọi API VietQR và trả về chuỗi QR
    public async Task<string> GenerateVietQrAsync(string cargoId, double amount)
    {
        using var client = new HttpClient();

        var payload = new
        {
            accountNo = 113366668888,
            accountName = "QUY VAC XIN PHONG CHONG COVID",
            acqId = 970415,
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
        string qrCode = obj.data.qrCode;

        return qrCode;
    }

    // Hàm publish lên MQTT, gọi GenerateVietQrAsync trước
    public async Task PublishOrderToMqttAsync(Product product)
    {
        var mqtt = new MqttService();
        await mqtt.ConnectAsync();

        string emvco = await GenerateVietQrAsync(product.Cargo_Id, product.Price);
        string payload = $"emvco={emvco}";
        await mqtt.PublishAsync(payload);

        await mqtt.DisconnectAsync();
        Console.WriteLine($"[MQTT] Published EMVCo for order {product.Cargo_Id}");
    }


    // Publish command to MQTT (servo_on / servo_off)
    public async Task PublishMqttCommandAsync(string command)
    {
        var mqtt = new MqttService();
        await mqtt.ConnectAsync();

        await mqtt.PublishAsync(command);

        await mqtt.DisconnectAsync();
        Console.WriteLine($"[MQTT] Published command: {command}");
    }


}

public class MqttService
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;

    private const string BrokerHost = "broker.hivemq.com";
    private const int BrokerPort = 1883;
    private const string DefaultClientId = "GiaohangbotApp";
    private const string DefaultTopic = "namha/iot";

    public MqttService(string clientId = DefaultClientId)
    {
        var factory = new MqttFactory();
        _client = factory.CreateMqttClient();

        _options = new MqttClientOptionsBuilder()
            .WithClientId(clientId)
            .WithTcpServer(BrokerHost, BrokerPort)
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
            .WithTopic(DefaultTopic)
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
