using Microservice.Seles.Context;
using Microservice.Seles.Models;
using Microservice.Seles.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Seles.Services
{
    public class BookRepository : IBookRepository
    {
        private SelesContext _context;
        public BookRepository(SelesContext context)
        {
            _context = context;
        }
        public async Task AddBook(Book book)
        {
            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var book = GetBookById(id);
            _context.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task EditBook(Book book)
        {
            _context.Entry(book).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public IEnumerable<Book> GetAllBook()
        {
            return _context.Books.OrderByDescending(b => b.Id);
        }

        public Book GetBookById(int? id)
        {
            return _context.Books.Find(id);
        }
    }
}
