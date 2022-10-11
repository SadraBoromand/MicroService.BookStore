using Microservice.BookStore.Models.DTOs;
using Microservice.BookStore.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Microservice.BookStore.Controllers
{
    public class AuthController : Controller
    {
        private IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            var user = await _userRepository.RegisterAsync(register);
            if (user != null)
            {
                ModelState.AddModelError("FullName", "کاربر با این اطلاعات موجود می باشد");
                return View(register);
            }

            return Redirect("/");
        }

        #region Login and Logout

        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync([Bind("Phone,Password,RemmberMe")] LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userRepository.Login(login);
            if (user == null)
            {
                ModelState.AddModelError("Email", "کاربری با اطلاعات فوق یافت نشد");
                return View(login);
            }

            #region Authentication

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName.ToLower()),
                new Claim(ClaimTypes.MobilePhone, user.Phone),
                new Claim(ClaimTypes.Hash, user.Password),
                new Claim(ClaimTypes.SerialNumber, user.Serial),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principle = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties()
            {
                IsPersistent = login.RemmberMe
            };

            await HttpContext.SignInAsync(principle, properties);

            #endregion

            return Redirect("/");
        }

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        #endregion
    }
}