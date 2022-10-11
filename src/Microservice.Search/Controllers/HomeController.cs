using Microservice.Search.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservice.Search.Controllers
{
    public class HomeController : Controller
    {
        public HomeController() { }
        public async Task<IActionResult> Index(string title)
        {
            GetApi getApi = new GetApi();
            ViewData["url"] = getApi._url;
            if (title == null || title.Length <= 0)
            {
                return View(await getApi.GetAllBook());
            }
            else
            {
                return View(await getApi.GetBookByTitle(title));
            }
        }

        public IActionResult Detail(Book book, string Price)
        {
            book.Price = decimal.Parse(Price);
            book.Image = new GetApi()._url + book.Image;
            return View(book);
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