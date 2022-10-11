using Microservice.Accounting.Context;
using Microservice.Accounting.Models.DTOs;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Accounting.Models
{
    public class ConsumerAccounting
    {
        private AccountingContext _context;
        public ConsumerAccounting(AccountingContext context)
        {
            _context = context;
        }
        /* 
         * consumer ex.Accounting
         * for cart item in microservice Seles to microservice Accounting
        */
        string hostName = "localhost";
        string userName = "guest";
        string password = "guest";
        string queueName = "Accounting";
        string exChange = "ex.Accounting";
        public void Consumer()
        {
            var factory = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "ex.Accounting",
                                        type: "topic",
                                        durable: true);


                channel.QueueBind(queue: queueName,
                                  exchange: exChange,
                                  routingKey: "#.Accounting");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);
            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var routingKey = e.RoutingKey;

            var consumer = JsonConvert.DeserializeObject<AccountingOrderUserViewModel>(message);
            var user = _context.Users.SingleOrDefault(u => u.Serial == consumer.UserSerial);
            var order = _context.Orders.SingleOrDefault(o => o.OrderSerial == consumer.OrderSerial);

            if (user == null)
            {
                user = new User()
                {
                    Serial = consumer.UserSerial,
                    FullName = consumer.FullName,
                    Password = consumer.Password,
                    Phone = consumer.Phone,
                    Role = consumer.Role,
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            if (order == null)
            {
                order = new Order()
                {
                    OrderSerial = consumer.OrderSerial,
                    UserSerial = user.Serial,
                    Price = consumer.Price,
                    IsPay = consumer.IsPay,
                    PayDate = consumer.PayDate
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
        }
    }
}
