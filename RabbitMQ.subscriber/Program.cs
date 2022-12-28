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


            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue",false,consumer);

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
