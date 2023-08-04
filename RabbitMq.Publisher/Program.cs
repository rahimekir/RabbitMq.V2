using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMq.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://kfsdoqic:jn0RezxxesWFhHedSXKfrxlcieC0PsC_@fish.rmq.cloudamqp.com/kfsdoqic");

            // factory üzerinden bağlantı açma
            using var connection = factory.CreateConnection();

            // bağlantıya  rabbitmq'ya kanal üzerinden bağlanma
            var channel = connection.CreateModel();

            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x=>
            {
                string message = $"Message {x} ";
                var messageBody = Encoding.UTF8.GetBytes(message);


                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

                Console.WriteLine($"Mesaj gönderilmiştir.: {message}");
            }
            );
            
            Console.ReadLine();
        }
    }
}
