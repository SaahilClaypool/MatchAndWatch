using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.UseCases.Genre;

using Infrastructure;
using Infrastructure.Models;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Shared.DTO.Login;

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
            var result = await SignInManager.PasswordSignInAsync(details.username, details.password, true, false);
            if (!result.Succeeded) {
                return new LoginResultDTO(false, "");
            }
            var user = await CurrentUserAccessor.FindByUsername(details.username);
            var token = GenerateJwtToken(user);
            Logger.LogDebug($"Logged in {details.username} with token {token}");

            return new LoginResultDTO(true, token);
        }

        [HttpPost("Logout")]
        public Task LogOut() {
            SignOut();
            return SignInManager.SignOutAsync();
        }


        private static string GenerateJwtToken(IUser user) {
            //https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("The quick brown foxes jumped over the lazy brown dogs");
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
