using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672/");
            using var connection= factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"Message {x}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(String.Empty, "hello-queue", null, messageBody);
                Console.WriteLine($"Mesaj gönderilmiştir: {message}");
                
            });

            Console.ReadLine();
        }
    }
}
