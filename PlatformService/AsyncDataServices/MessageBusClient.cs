using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using PlatformService.Dtos;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        public MessageBusClient(IConfiguration configuration) 
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
            };
            try
            {
                _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
                _channel = _connection.CreateChannelAsync().Result;
                _channel.ExchangeDeclareAsync(exchange: "trigger", type: ExchangeType.Fanout).GetAwaiter().GetResult();
                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> Conneted to messageBus");
            }
            catch (Exception ex)
            { 
                Console.WriteLine($"--> Could not connect to the message Bus: {ex.Message}");
            }
        }
        public async Task PublishPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if (_connection.IsOpen == true)
            {
                Console.WriteLine($"RabbitMQ Connection open, sending message {message}");
                // to do send the message
                await SendMessage(message);
            }
            {
                Console.WriteLine($"RabbitMQ Connection closed, not sending {_connection.IsOpen}");
            }
        }

        private async Task SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var props = new BasicProperties();

            await _channel.BasicPublishAsync(
                exchange: "trigger", 
                routingKey: "", 
                mandatory: true, 
                basicProperties: props, 
                body: body);

            Console.WriteLine($"--> we have sent {message}");
        }
        public async Task Dispose()
        {
            Console.WriteLine("Message Bus disposed");
            if (_channel.IsOpen)
            {
                await _channel.CloseAsync();
                await _connection.CloseAsync();
            }
        }

        public async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
            if (e.Cause != null)
            {
                Console.WriteLine($"Cause: {e.Cause}");
            }
            await Task.Delay(500);
        }
    }
}
