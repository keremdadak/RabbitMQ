using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace RabbitMQ.subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://guest:guest@localhost:5672/");
            using var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //random kuyruk ismi oluşturma
            var randomQueueName = "log-database-save-queue";//channel.QueueDeclare().QueueName;
            channel.QueueDeclare(randomQueueName, true, false, false);
            channel.QueueBind(randomQueueName, "logs-fanout", "", null);

            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume(randomQueueName, false,consumer);
            Console.WriteLine("Loglar Dinleniyor...");
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Thread.Sleep(1500);
                Console.WriteLine("Gelen mesaj :" + message);

                channel.BasicAck(e.DeliveryTag, false);
            };
            Console.ReadLine();
        }
        
        #region Ayrı bir method ile yapılırsa received böyle oluyor.
        //private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        //{
        //    throw new NotImplementedException();
        //} 
        #endregion
    }
}
