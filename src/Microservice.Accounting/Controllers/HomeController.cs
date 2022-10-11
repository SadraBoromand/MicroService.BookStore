using Microservice.Accounting.Context;
using Microservice.Accounting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microservice.Accounting.Models.DTOs;

namespace Microservice.Accounting.Controllers
{
    public class HomeController : Controller
    {
        private AccountingContext _context;

        public HomeController(AccountingContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            new ConsumerAccounting(_context).Consumer();
            var result = _context.Orders
                .Select(o => new AccountingListViewModel()
                {
                    // for order
                    OrderId = o.OrderId,
                    OrderSerial = o.OrderSerial,
                    IsPay = o.IsPay,
                    PayDate = o.PayDate,
                    Price = o.Price,
                    UserSerial = o.UserSerial,
                    // for user
                    FullName = _context.Users.SingleOrDefault(u => u.Serial == o.UserSerial).FullName,
                    Role = _context.Users.SingleOrDefault(u => u.Serial == o.UserSerial).Role,
                });
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}