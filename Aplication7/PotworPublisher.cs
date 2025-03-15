﻿using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Domain7;

namespace Aplication7
{
    public class PotworPublisher
    {

        private readonly IChannel _channel;

        public PotworPublisher(IChannel channel)
        {
            _channel = channel;
        }

        public void Publish<T>(T eventMessage)
        {
            string queueName = GetQueueName(typeof(T));
            _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            string messageBody = JsonConvert.SerializeObject(eventMessage);
            var body = Encoding.UTF8.GetBytes(messageBody);

            _channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
            Console.WriteLine($"[x] Opublikowano wiadomość do kolejki '{queueName}': {messageBody}");
        }

        private static string GetQueueName(Type eventType)
        {
            return eventType.Name.Replace("Event", "").ToLower();
        }


        public void Run()
        {

            while (true)
            {
                Publish(new ZapisPotworaEvent()
                {
                    EventId = Guid.NewGuid(),
                    Timestamp = DateTime.UtcNow,
                    Data = "Dane postaci"
                });


                Thread.Sleep(10000);
            }

        }
    }


}
