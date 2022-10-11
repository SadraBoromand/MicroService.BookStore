using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microservice.Accounting.Context;
using Microservice.Accounting.Models;
using Microservice.Accounting.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CodeActions;

namespace Microservice.Accounting.Controllers
{
    public class AuthController : Controller
    {
        private AccountingContext _context;

        public AuthController(AccountingContext context)
        {
            _context = context;
        }

        [Route("Login")]
        public IActionResult Login()
        {
            new ConsumerAccounting(_context).Consumer();
            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Phone,Password,RemmberMe")]
                                                LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _context.Users
                .SingleOrDefault(u => u.Phone == login.Phone &&
                                u.Password == login.Password);
            if (user == null)
            {
                ModelState.AddModelError("Phone", "کاربر مورد نظر یافت نشد");
                return View(login);
            }

            #region Authentication

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Serial),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults
                                                            .AuthenticationScheme);
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
            return RedirectToAction("Login");
        }
    }
}