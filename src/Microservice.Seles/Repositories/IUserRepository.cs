using Microservice.Seles.Models;
using Microservice.Seles.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Seles.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUser();
        User GetUserById(string serial);
        User GetUserById(int id);
        User AddUser(User user);
        bool EditUser(User user);
        bool DeleteUser(int id);

        // Login
        User Login(LoginViewModel login);
    }
}
