
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace Aplication4
{
    public class RunPublisher
    {


        public  async void Main(string[] args)
        {


            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information); // Minimalny poziom logowania
            });

            // Tworzenie loggera dla głównego programu
            var logger = loggerFactory.CreateLogger<RunPublisher>();

            // Przykładowe logowanie
            logger.LogInformation("Aplikacja została uruchomiona.");

            // Konfiguracja fabryki połączeń
            var factory = new ConnectionFactory()
            {
            };
            factory.Uri = new Uri("amqps://dvdrazhu:0ufAa9AIPEuI9hHUq68SpLnKAZIgrDlv@cow.rmq2.cloudamqp.com/dvdrazhu");
            // Nawiązanie połączenia z RabbitMQ
            using (var connection = await factory.CreateConnectionAsync())
            {
                // Utworzenie kanału komunikacyjnego
                using (var channel = await connection.CreateChannelAsync())
                {

                    GraczPublisher graczPublisher = new GraczPublisher(channel);
                    graczPublisher.Run();
                }
            }
        }
    }


}