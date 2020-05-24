using Cover.Domain.Models;
using Cover.Domain.Services;
using Cover.Models;
using Cover.Neo4J.Domain.Core.Repository;
using Cover.Neo4J.Domain.Core.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Sparkle.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserService _service;
        private readonly UserRepository _userRepository;
        public AccountController(UserService service, UserRepository userRepository)
        {
            _service = service;
            _userRepository = userRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _service.GetByLoginAndPasswordAsync(model.Login, model.Password);
                if (user != null)
                {
                    await Authenticate(model.Login);
                    return RedirectToAction("News", "Home");
                }
                ModelState.AddModelError(nameof(LoginViewModel.Login), "Invalid username or password");
            }
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Debug.WriteLine($"Logout method: is auth {User.Identity.IsAuthenticated}");
            return RedirectToAction("Login", "Account");
        }



        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _service.GetByLoginAsync(model.Login);
                if (user == null)
                {
                    user = new User()
                    {
                        Email = model.Email,
                        Login = model.Login,
                        Age = DateTime.Now.Year - Convert.ToInt32(model.DateOfBirth.Year),
                        Password = model.Password,
                        Name = model.Name,
                        Surname = model.Surname,
                        PostIds = new List<string>(),
                        DateOfBirth = model.DateOfBirth,
                        Friends = new List<Friend>()
                    };
                    await _service.CreateAsync(user);
                    _userRepository.Add(user);

                    await Authenticate(model.Login);

                    return RedirectToAction("News", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login and(or) password");
                }
            }
            return View(model);
        }

        private async Task Authenticate(string login)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, login),
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}