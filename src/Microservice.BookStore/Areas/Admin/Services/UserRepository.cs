using Microservice.BookStore.Context;
using Microservice.BookStore.Models;
using Microservice.BookStore.Models.DTOs;
using Microservice.BookStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.BookStore.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly BookStoreContext _context;
        public UserRepository(BookStoreContext context)
        {
            _context = context;
        }
        public async Task<bool> AddUser(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            try
            {
                var user = await GetUserById(id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool EditUser(User user)
        {
            try
            {
                _context.Entry(user).State = Microsoft
                    .EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<User> GetAllUser()
        {
            return _context.Users.OrderByDescending(u => u.Id);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public User Login(LoginViewModel login)
        {
            return _context.Users
                .FirstOrDefault(u => u.Phone == login.Phone && u.Password == login.Password);
        }

        public async Task<User> RegisterAsync(RegisterViewModel register)
        {
            var user = _context.Users.SingleOrDefault(u => u.Phone == register.Phone);
            if (user != null)
            {
                return user;
            }

            user = new User()
            {
                Serial = Guid.NewGuid().ToString(),
                FullName = register.FullName,
                Phone = register.Phone,
                Password = register.Password,
                Role = "user"
            };
            await AddUser(user);

            return null;
        }
    }
}
