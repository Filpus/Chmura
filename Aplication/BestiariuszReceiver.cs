using Domain1;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication1
{
    public class BestiariuszReceiver
    {
        private readonly IChannel _channel;

        public BestiariuszReceiver(IChannel channel)
        {
            _channel = channel;
        }

        public void StartConsuming()
        {
            string queueName = GetQueueName(typeof(ZapisPotworaEvent));
            _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Odebrano wiadomość z kolejki '{queueName}': {message}");

                return Task.CompletedTask;
            };

            _channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($"[x] Subskrybent nasłuchuje na kolejce '{queueName}'");
        }

        private static string GetQueueName(Type eventType)
        {
            return eventType.Name.Replace("Event", "").ToLower();
        }

        public void Run()
        {
            StartConsuming();
            Console.WriteLine("Nasłuchiwanie rozpoczęte. Naciśnij dowolny klawisz, aby zakończyć.");
            Console.ReadKey();
        }
    }
}
