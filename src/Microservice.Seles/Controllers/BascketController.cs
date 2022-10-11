using Microservice.Seles.Context;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microservice.Seles.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microservice.Seles.Models.DTOs;

namespace Microservice.Seles.Controllers
{
    public class BascketController : Controller
    {
        private SelesContext _context;

        public BascketController(SelesContext context)
        {
            _context = context;
        }

        public IActionResult Payment(string userSerial, int orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .SingleOrDefault(o => o.OrderId == orderId);

            string message = "";

            if (order != null)
            {
                order.IsPay = true;

                var accountingOrderUserViewMode = 
                    new AccountingOrderUserViewModel()
                {
                    // user
                    UserSerial = User.FindFirstValue(ClaimTypes.SerialNumber),
                    FullName = User.FindFirstValue(ClaimTypes.Name),
                    Role = User.FindFirstValue(ClaimTypes.Role),
                    Phone = User.FindFirstValue(ClaimTypes.MobilePhone),
                    Password = User.FindFirstValue(ClaimTypes.Hash),
                    // order
                    OrderId = order.OrderId.ToString(),
                    OrderSerial=order.OrderSerial,
                    Price = order.OrderItems.Sum(oi => oi.Price),
                    IsPay = order.IsPay,
                    PayDate = DateTime.Now
                };

                message = JsonConvert
                    .SerializeObject(accountingOrderUserViewMode);
            }

            var accounting = new ProducerRabbitMq();
            accounting.ProducerAccounting("Add.Accounting", message);

            //_context.SaveChanges();
            return View("Payment", order.OrderSerial);
        }

        public IActionResult RemoveOrderItem(int id)
        {
            var orderItem = _context.OrderItems.Find(id);
            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
            return Redirect("/");
        }
    }
}