using Microservice.Seles.Context;
using Microservice.Seles.Models.DTOs;
using Microservice.Seles.Repositories;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Seles.Models
{
    public class ConsumerSeles
    {
        private IUserRepository _userRepository;
        private IBookRepository _bookRepository;
        private SelesContext _context;
        public ConsumerSeles(SelesContext context, 
            IUserRepository userRepository, 
            IBookRepository bookRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }


        string hostName = "localhost";
        string userName = "guest";
        string password = "guest";
        string queueName = "Seles";
        string exChange = "ex.Seles";
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
                channel.ExchangeDeclare(exchange: exChange,
                                        type: "topic",
                                        durable: true);

                channel.QueueBind(queue: queueName,
                                  exchange: exChange,
                                  routingKey: "Seles.#");


                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;
                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

            }
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.ToArray());
            SelesViewModel selesViewModel = JsonConvert.DeserializeObject<SelesViewModel>(message);
            var user = _userRepository
                .GetUserById(selesViewModel.UserSerial);
            var book = _context.Books
                .SingleOrDefault(b => b.Serial == selesViewModel.BookSerial);
            var order = _context.Orders
                .SingleOrDefault(o => o.UserSerial == selesViewModel.UserSerial &&
                                      o.IsPay == false);

            if (user == null)
            {
                user = new User()
                {
                    Serial = selesViewModel.UserSerial,
                    FullName = selesViewModel.FullName,
                    Phone = selesViewModel.Phone,
                    Password = selesViewModel.Password,
                    Role = selesViewModel.Role
                };
                _context.Users.Add(user);
                _context.SaveChanges();
            }
            if (book == null)
            {
                book = new Book()
                {
                    Serial = selesViewModel.BookSerial,
                    Title = selesViewModel.Title,
                    Auther = selesViewModel.Auther,
                    Description = selesViewModel.Description,
                    Price = selesViewModel.Price,
                    Image = selesViewModel.Image,
                    Count = selesViewModel.Count
                };
                _context.Books.Add(book);
                _context.SaveChanges();
            }
            if (order == null)
            {
                order = new Order()
                {
                    UserSerial = user.Serial,
                    OrderSerial = Guid.NewGuid().ToString(),
                    IsPay = false
                };
                _context.Orders.Add(order);
                _context.SaveChanges();

                var orderItem = new OrderItem()
                {
                    OrderId = order.OrderId,
                    BookId = book.Id,
                    Count = selesViewModel.Count,
                    Price = book.Price
                };
                _context.OrderItems.Add(orderItem);
                _context.SaveChanges();
            }
            else
            {
                var orderItem = _context.OrderItems
                    .SingleOrDefault(oi => 
                oi.OrderId == order.OrderId && 
                oi.BookId == book.Id);
                if (orderItem == null)
                {
                    orderItem = new OrderItem()
                    {
                        OrderId = order.OrderId,
                        BookId = book.Id,
                        Count = selesViewModel.Count,
                        Price = book.Price
                    };
                    _context.OrderItems.Add(orderItem);
                }
                else
                {
                    orderItem.Count += selesViewModel.Count;
                    orderItem.Price = orderItem.Count * book.Price;
                    _context.OrderItems.Update(orderItem);
                }
                _context.SaveChanges();
            }

        }
    }
}
