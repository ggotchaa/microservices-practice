using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class BlogCommunicationService : IBlogCommunicationService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _weatherQueue = "weather_queue";

    public BlogCommunicationService(IConfiguration configuration)
    {
        var rabbitMQHost = configuration["RabbitMQ:HostName"];
        var rabbitMQUsername = configuration["RabbitMQ:UserName"];
        var rabbitMQPassword = configuration["RabbitMQ:Password"];

        var factory = new ConnectionFactory
        {
            HostName = rabbitMQHost,
            UserName = rabbitMQUsername,
            Password = rabbitMQPassword
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _weatherQueue, durable: false, exclusive: false, autoDelete: false, arguments: null);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += OnWeatherMessageReceived;
        _channel.BasicConsume(queue: _weatherQueue, autoAck: true, consumer: consumer);
    }

    public Task StartConsuming()
    {
        // Start consuming RabbitMQ messages
        return Task.CompletedTask;
    }

    public Task StopConsuming()
    {
        // Stop consuming RabbitMQ messages
        _connection.Close();
        return Task.CompletedTask;
    }

    private void OnWeatherMessageReceived(object sender, BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);

        // Deserialize the message to get latitude and longitude
        // Fetch weather data from Weather API based on latitude and longitude
        // Process the weather data as needed

        Console.WriteLine($"Weather data received: {message}");
    }
}
