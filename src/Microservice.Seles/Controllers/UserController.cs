using Microservice.Seles.Context;
using Microservice.Seles.Models;
using Microservice.Seles.Models.DTOs;
using Microservice.Seles.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Seles.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        private IBookRepository _bookRepository;
        private SelesContext _context;
        public UserController(SelesContext context, IUserRepository userRepository, IBookRepository bookRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _bookRepository = bookRepository;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            new ConsumerSeles(_context, _userRepository, _bookRepository).Consumer();

            return View();
        }

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync([Bind("Phone,Password,RemmberMe")]LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userRepository.Login(login);
            if (user == null)
            {
                ModelState.AddModelError("All", "کاربر با اطلاعات وارد شده هنوز عضو سیستم نشده است");
                return View(login);
            }

            #region Authentication

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.FullName.ToLower()),
                new Claim(ClaimTypes.MobilePhone,user.Phone),
                new Claim(ClaimTypes.Hash,user.Password),
                new Claim(ClaimTypes.SerialNumber,user.Serial),
                new Claim(ClaimTypes.Role,user.Role.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties()
            {
                IsPersistent = login.RemmberMe
            };

            await HttpContext.SignInAsync(principle, properties);

            #endregion

            return Redirect("/Home/Index");
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }


        #region RabbitMq Consumer

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
