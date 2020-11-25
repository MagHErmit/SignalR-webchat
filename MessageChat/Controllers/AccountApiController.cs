using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MessageChat.DataRepositories;
using MessageChat.DomainModels;
using MessageChat.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageChat.Controllers
{
    [Route("account")]
    public class AccountApiController : ControllerBase
    {
        private readonly IAccountRepository _users;

        public AccountApiController(IAccountRepository users)
        {
            _users = users;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInDto loginData)
        {
            //return BadRequest("Введен некорректный логин");
            if (string.IsNullOrWhiteSpace(loginData.Name) || loginData.Name.Length < 4 || loginData.Name.Length > 20)
                return BadRequest("Введен некорректный логин");
            
            var user = _users.GetUser(loginData.Name);
            if (user.Password != loginData.Password)
                return Unauthorized("Неверный пароль");
            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
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
                identificator = claims[0].Value,
                password = loginData.Password
            });
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationDto regData)
        {
            if (string.IsNullOrWhiteSpace(regData.Name) || regData.Name.Length < 4 || regData.Name.Length > 20)
                return BadRequest("Введенно некорректное имя пользователя");

            _users.RegisterUser(new UserModel(Guid.NewGuid().ToString(), regData));
            return new EmptyResult();
        }

        [HttpPost("logout"), Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return new EmptyResult();
        }
    }
}