using GameLive.Web.Hubs;
using GameLive.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GameLive.Web.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(Logic.AccountLogic accountLogic)
        {
            _accountLogic = accountLogic;
        }

        private readonly Logic.AccountLogic _accountLogic;

        /// <summary>
        /// Произвести логин
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login()
        {
            User user;
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                user = new Models.User() { Id = _accountLogic.GenerateCode() };
                await Authenticate(user.Id);
            }
            else
            {
                if (_accountLogic.Validate(User.Identity.Name))
                { user = new Models.User() { Id = User.Identity.Name }; }
                else
                { user = null; }
            }

            return Json(new AccountLoginResult() { IsLogined = user != null });
        }

        private async Task Authenticate(String userID)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userID)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}