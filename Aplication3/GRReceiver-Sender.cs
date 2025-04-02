using Domain3;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


using System.Text;

namespace Aplication3
{
    public class GRReceiveSender
    {
        private readonly IChannel _channel;

        public GRReceiveSender(IChannel channel)
        {
            _channel = channel;
        }

        private void StartConsuming()
        {
            string queueName = GetQueueName(typeof(ProbaAtakuEvent));
            _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Odebrano wiadomość z kolejki '{queueName}': {message}");
                Logger.Log($"[x] Odebrano wiadomość z kolejki '{queueName}': {message}");
                Publish(new UdanyAtakEvent()
                {
                    EventId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow,
                    Data = "Dane ataku"
                });
                return Task.CompletedTask;
            };

            _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($"[x] Subskrybent nasłuchuje na kolejce '{queueName}'");
            Logger.Log($"[x] Subskrybent nasłuchuje na kolejce '{queueName}'");
        }

        private void Publish<T>(T eventMessage)
        {
            string queueName = GetQueueName(typeof(T));
            _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            string messageBody = JsonConvert.SerializeObject(eventMessage);
            var body = Encoding.UTF8.GetBytes(messageBody);

            _channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
            Console.WriteLine($"[x] Opublikowano wiadomość do kolejki '{queueName}': {messageBody}");
            Logger.Log($"[x] Opublikowano wiadomość do kolejki '{queueName}': {messageBody}");
        }


        private static string GetQueueName(Type eventType)
        {
            return eventType.Name.Replace("Event", "").ToLower();
        }

        public void Run()
        {
            StartConsuming();
            Console.WriteLine("Nasłuchiwanie rozpoczęte. Naciśnij dowolny klawisz, aby zakończyć.");
            Logger.Log("Nasłuchiwanie rozpoczęte. Naciśnij dowolny klawisz, aby zakończyć.");
            Console.ReadKey();
        }
    }
    class Logger
    {
        private static readonly string logFilePath = "log.txt";

        public static void Log(string message)
        {
            using (StreamWriter writer = File.AppendText(logFilePath))
            {
                writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
            }
        }
    }
}
