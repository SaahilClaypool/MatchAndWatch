using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;

using DTO.Login;

using Infrastructure.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

// https://devblogs.microsoft.com/aspnet/asp-net-core-authentication-with-identityserver4/
// has example of the message type needed to get token from identityserver4
namespace App.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase {
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly ILogger Logger;
        private ICurrentUserAccessor CurrentUserAccessor { get; init; }

        public LoginController(SignInManager<ApplicationUser> signInManager, ILogger<LoginController> logger, ICurrentUserAccessor currentUserAccessor) {
            SignInManager = signInManager;
            Logger = logger;
            CurrentUserAccessor = currentUserAccessor;
        }

        [HttpPost]
        public async Task<LoginResultDTO> LogIn(
            [FromBody] LoginDTO details
        ) {
            var result = await SignInManager.PasswordSignInAsync(details.Username, details.Password, true, false);
            if (!result.Succeeded) {
                return new LoginResultDTO(false, "");
            }
            var user = await CurrentUserAccessor.FindByUsername(details.Username);
            var token = JwtMiddleware.GenerateJwtToken(user, Logger);
            Logger.LogDebug($"Logged in {details.Username} with token {token}");

            return new LoginResultDTO(true, token);
        }

        [HttpPost("Logout")]
        public Task LogOut() {
            SignOut();
            return SignInManager.SignOutAsync();
        }


    }
}
