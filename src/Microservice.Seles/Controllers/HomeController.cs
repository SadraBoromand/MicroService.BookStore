using Microservice.Seles.Context;
using Microservice.Seles.Models;
using Microservice.Seles.Models.DTOs;
using Microservice.Seles.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Seles.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IUserRepository _userRepository;
        private IBookRepository _bookRepository;
        private SelesContext _context;
        public HomeController(SelesContext context, IUserRepository userRepository, IBookRepository bookRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            new ConsumerSeles(_context,_userRepository,_bookRepository).Consumer();

            //SelesConsumer();
            var order = _context.Orders
                  .Include(o => o.OrderItems)
                  .Select(o => new OrderViewModel()
                  {
                      OrderItems = _context.OrderItems.Select(oi => new OrderItemViewModel()
                      {
                          OrderItemId = oi.OrderItemId,
                          OrderId = oi.OrderId,
                          BookId = oi.BookId,
                          BookTitle = _context.Books.SingleOrDefault(b => b.Id == oi.BookId).Title,
                          Count = oi.Count,
                          Price = oi.Price
                      })
                      .Where(oi => oi.OrderId == o.OrderId)
                      .ToList(),
                      IsPay = o.IsPay,
                      OrderId = o.OrderId,
                      UserSerial = o.UserSerial
                  })
                  .SingleOrDefault(o => o.IsPay == false);
            return View(order);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
/*        #region RabbitMq Consumer

        private void SelesConsumer()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            string queue = "Seles";

            channel.QueueDeclare(
                queue: queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);

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
                var orderItem = _context.OrderItems.SingleOrDefault(oi => oi.OrderId == order.OrderId && oi.BookId == book.Id);
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
        #endregion
    }
}
*/