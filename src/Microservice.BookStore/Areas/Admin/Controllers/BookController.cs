using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microservice.BookStore.Context;
using Microservice.BookStore.Models;
using Microservice.BookStore.Models.DTOs;
using Microservice.BookStore.Repositories;

namespace Microservice.BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        // GET: Book
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

        // GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Auther,Price,Count,Image")]
            AddEditBookViewModel book)
        {
            if (ModelState.IsValid)
            {
                var saveImage = new SaveImage();
                string image = saveImage.Save(book.Image);

                var book1 = new Book()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    Auther = book.Auther,
                    Price = book.Price,
                    Image = image,
                    Count = book.Count,
                    Serial = Guid.NewGuid().ToString()
                };
                await _bookRepository.AddBook(book1);
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Book/Edit/5
        public IActionResult Edit(int? id)
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

            return View(new AddEditBookViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                Auther = book.Auther,
                Description = book.Description,
                OldImage = book.Image,
                Price = book.Price,
                Count = book.Count
            });
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Auther,Price,Image,Count,OldImage")]
            AddEditBookViewModel book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string image = book.OldImage;
                    if (book.Image != null)
                    {
                        var saveImage = new SaveImage();
                        saveImage.Delete(book.OldImage);
                        image = saveImage.Save(book.Image);
                    }

                    var book1 = new Book()
                    {
                        Id = book.Id,
                        Title = book.Title,
                        Description = book.Description,
                        Auther = book.Auther,
                        Price = book.Price,
                        Image = image,
                        Count = book.Count
                    };
                    await _bookRepository.EditBook(book1);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_bookRepository.GetBookById(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
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

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookRepository.DeleteBook(id);
            return RedirectToAction(nameof(Index));
        }
    }
}