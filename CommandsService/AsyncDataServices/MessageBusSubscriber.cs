
using System.Text;
using System.Threading.Channels;
using CommandsService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IChannel _channel;
        private QueueDeclareOk _queuName;
        public MessageBusSubscriber(
            IConfiguration configuration, 
            IEventProcessor eventProcessor) 
        { 
            _configuration = configuration;
            _eventProcessor = eventProcessor;
            initializeRabitMQ().GetAwaiter().GetResult();
        }
        private async Task initializeRabitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"]),
            };
            try
            {
                _connection =  await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                await _channel.ExchangeDeclareAsync(
                    exchange: "trigger",
                    type: ExchangeType.Fanout);
                _queuName = await _channel.QueueDeclareAsync();
                await _channel.QueueBindAsync(
                    queue: _queuName.QueueName,
                    exchange: "trigger",
                    routingKey: ""
                    );

                Console.WriteLine("--> Listening on the message Bus...");
                _connection.ConnectionShutdownAsync += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message Bus: {ex.Message}");
            }
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (moduleHandle, e) =>
            {
                Console.WriteLine("--> Event Received");
                var body = e.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.processEvent(notificationMessage);
                await Task.FromResult(true);
            };
             _channel.BasicConsumeAsync(
                queue: _queuName.QueueName,
                autoAck: true,
                consumer: consumer);
            return Task.CompletedTask;
        }

        public async Task RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
            if (e.Cause != null)
            {
                Console.WriteLine($"Cause: {e.Cause}");
            }
            await Task.FromResult(true);
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.CloseAsync().GetAwaiter().GetResult();
                _connection.CloseAsync().GetAwaiter().GetResult();
            }
            base.Dispose();
        }
    }
}
