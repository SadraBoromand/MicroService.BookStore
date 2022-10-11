using Microservice.BookStore.Context;
using Microservice.BookStore.Models;
using Microservice.BookStore.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.BookStore.Services
{
    public class BookRepository : IBookRepository
    {
        private BookStoreContext _context;
        public BookRepository(BookStoreContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBook()
        {
            return _context.Books.OrderByDescending(b => b.Id);
        }
        public IEnumerable<Book> GetBookByTitle(string title)
        {
            return _context.Books.Where(b => b.Title.Contains(title) ||
            b.Description.Contains(title) ||
            b.Auther.Contains(title));
        }

        public async Task AddBook(Book book)
        {
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var book = GetBookById(id);
            new SaveImage().Delete(book.Image);
            _context.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task EditBook(Book book)
        {
            _context.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        public Book GetBookById(int? id)
        {
            return _context.Books.Find(id);
        }

    }
}
