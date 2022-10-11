using Microservice.Seles.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Seles.Repositories
{
    public interface IBookRepository
    {
        IEnumerable<Book> GetAllBook();
        Book GetBookById(int? id);
        Task AddBook(Book book);
        Task EditBook(Book book);
        Task DeleteBook(int id);
    }
}