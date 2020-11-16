using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MessageChat.Dto;
using MessageChat.AccountRepository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageChat.Controllers
{
    [Route("account")]
    public class AccountApiController : ControllerBase
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInDto loginData)
        {
            if (string.IsNullOrWhiteSpace(loginData.Name) || loginData.Name.Length < 4 || loginData.Name.Length > 20)
                return BadRequest("Введен некорректный логин");

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, loginData.Name), 
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTime.UtcNow.Add(TimeSpan.FromHours(24))
            });

            return new ObjectResult( new 
            {
                name = loginData.Name,
                identificator = claims[0].Value
            });
        }

        [HttpPost("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new EmptyResult();
        }
    }
}