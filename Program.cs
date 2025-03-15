using RabbitMQ.Client;
using System.Text;

namespace Aplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Tworzenie fabryki po��cze�
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://dvdrazhu:0ufAa9AIPEuI9hHUq68SpLnKAZIgrDlv@cow.rmq2.cloudamqp.com/dvdrazhu")
            };

            // Tworzenie po��czenia
            using (IConnection connection = factory.CreateConnection())
            {
                // Tworzenie kana�u
                using (IModel channel = connection.CreateModel())
                {
                    // Deklaracja kolejki
                    channel.QueueDeclare(queue: "exampleQueue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    // Publikowanie wiadomo�ci
                    string message = "Hello, RabbitMQ!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "exampleQueue",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($"[x] Wys�ano wiadomo��: {message}");
                }
            }
        }
    }
}
