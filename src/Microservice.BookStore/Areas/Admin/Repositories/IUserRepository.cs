using Microservice.BookStore.Models;
using Microservice.BookStore.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.BookStore.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        Task<User> GetUserById(int id);
        Task<bool> AddUser(User user);
        bool EditUser(User user);
        Task<bool> DeleteUser(int id);

        // Login and Register
        User Login(LoginViewModel login);
        Task<User> RegisterAsync(RegisterViewModel register);
    }
}
