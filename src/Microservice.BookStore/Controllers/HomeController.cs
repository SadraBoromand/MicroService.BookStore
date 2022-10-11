using Microservice.BookStore.Models;
using Microservice.BookStore.Models.DTOs;
using Microservice.BookStore.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microservice.BookStore.Controllers
{
    public class HomeController : Controller
    {
        private IBookRepository _bookRepository;

        public HomeController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public IActionResult Index()
        {
            return View(_bookRepository.GetAllBook());
        }

        // GET: Book/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // For ex.change ex.Seles
        public IActionResult Seles(Book book)
        {
            var selesViewModel = new SelesViewModel()
            {
                // book
                BookSerial = book.Serial,
                Title = book.Title,
                Auther = book.Auther,
                Description = book.Description,
                Count = book.Count,
                Price = book.Price,
                Image = book.Image,
                // user
                UserSerial = User.FindFirstValue(ClaimTypes.SerialNumber),
                FullName = User.FindFirstValue(ClaimTypes.Name),
                Role = User.FindFirstValue(ClaimTypes.Role),
                Phone = User.FindFirstValue(ClaimTypes.MobilePhone),
                Password = User.FindFirstValue(ClaimTypes.Hash)
            };
            var message = JsonConvert.SerializeObject(selesViewModel);
            // Producer to exchange ex.Seles
            new ProducerRabbitMq().SeleBook(routingKey: "Seles.AddSeles", message);
            return Redirect("/");
        }

        // Api Get List All Book
        [HttpGet("api/GetAllbook")]
        public string ApiGetAllBook()
        {
            var books = _bookRepository.GetAllBook();
            var result = JsonConvert.SerializeObject(books);
            return result;
        }
        // Api Search by Book Title and Auther and Desc Book
        [HttpGet("api/GetBook/{title}")]
        public string ApiGetBookById([FromRoute] string title)
        {
            return JsonConvert.SerializeObject(_bookRepository.GetBookByTitle(title));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}