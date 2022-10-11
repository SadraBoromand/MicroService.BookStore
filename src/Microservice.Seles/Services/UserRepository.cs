using Microservice.Seles.Context;
using Microservice.Seles.Models;
using Microservice.Seles.Models.DTOs;
using Microservice.Seles.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.Seles.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly SelesContext _context;
        public UserRepository(SelesContext context)
        {
            _context = context;
        }
        public User AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChangesAsync();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                var user = GetUserById(id);
                _context.Users.Remove(user);
                _context.SaveChanges();
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
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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

        public User GetUserById(string serial)
        {
            return _context.Users.SingleOrDefault(u => u.Serial == serial);
        }

        public User GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public User Login(LoginViewModel login)
        {
            return _context.Users
                .FirstOrDefault(u => u.Phone == login.Phone && u.Password == login.Password);
        }
    }
}
