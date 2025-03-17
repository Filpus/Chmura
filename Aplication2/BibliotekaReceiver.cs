using Domain2;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication2
{
    public class BibliotekaReceiver
    {
        private readonly IChannel _channel;

        public BibliotekaReceiver(IChannel channel)
        {
            _channel = channel;
        }

        public void StartConsuming()
        {
            string queueName = GetQueueName(typeof(ZapisPostaciEvent));
            _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Odebrano wiadomość z kolejki '{queueName}': {message}");
                Logger.Log($"[x] Odebrano wiadomość z kolejki '{queueName}': {message}");

                return Task.CompletedTask;
            };

            _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($"[x] Subskrybent nasłuchuje na kolejce '{queueName}'");
            Logger.Log($"[x] Subskrybent nasłuchuje na kolejce '{queueName}'");
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
