using Microservice.BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.BookStore.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBook();
        Book GetBookById(int? id);
        IEnumerable<Book> GetBookByTitle(string title);
        Task AddBook(Book book);
        Task EditBook(Book book);
        Task DeleteBook(int id);
    }
}