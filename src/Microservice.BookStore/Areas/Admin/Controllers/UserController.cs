using Microservice.BookStore.Models;
using Microservice.BookStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservice.BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View(_userRepository.GetAllUser());
        }

        #region Add
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(User user)
        {
            user.Serial = Guid.NewGuid().ToString();
            await _userRepository.AddUser(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit
        public async Task<IActionResult> EditUser(int id)
        {
            return View(await _userRepository.GetUserById(id));
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            _userRepository.EditUser(user);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        public async Task<IActionResult> DeleteUser(int id)
        {
            return View(await _userRepository.GetUserById(id));
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUser(id);
            return RedirectToAction("Index");
        }
        #endregion
    }
}
