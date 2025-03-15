using RabbitMQ.Client;
using System.Text;

namespace Aplication
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Tworzenie fabryki po³¹czeñ
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqps://dvdrazhu:0ufAa9AIPEuI9hHUq68SpLnKAZIgrDlv@cow.rmq2.cloudamqp.com/dvdrazhu")
            };

            // Tworzenie po³¹czenia
            using (IConnection connection = factory.CreateConnection())
            {
                // Tworzenie kana³u
                using (IModel channel = connection.CreateModel())
                {
                    // Deklaracja kolejki
                    channel.QueueDeclare(queue: "exampleQueue",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    // Publikowanie wiadomoœci
                    string message = "Hello, RabbitMQ!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "exampleQueue",
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($"[x] Wys³ano wiadomoœæ: {message}");
                }
            }
        }
    }
}
