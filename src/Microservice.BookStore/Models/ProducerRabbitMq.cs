using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.BookStore.Models
{
    public class ProducerRabbitMq
    {

        private const string UName = "guest";
        private const string PWD = "guest";
        private const string HName = "localhost";

        // routing Key for Queue Seles => Seles.#
        public void SeleBook(string routingKey, string message)
        {
            //Main entry point to the RabbitMQ .NET AMQP client
            var connectionFactory = new ConnectionFactory()
            {
                UserName = UName,
                Password = PWD,
                HostName = HName
            };

            var connection = connectionFactory.CreateConnection();
            var model = connection.CreateModel();
            var properties = model.CreateBasicProperties();

            properties.Persistent = false;

            byte[] messagebuffer = Encoding.UTF8.GetBytes(message);
            // exchange topic ex.Seles
            model.BasicPublish(exchange: "ex.Seles",
                routingKey: routingKey,
                properties,
                messagebuffer);


        }
    }
}
