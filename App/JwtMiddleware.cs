using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace App {
    public class JwtMiddleware {
        private readonly RequestDelegate Next;
        private readonly ILogger<JwtMiddleware> Logger;
        private const string Secret = "The quick brown foxes jumped over the lazy brown dogs";

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger) {
            Next = next;
            Logger = logger;
        }

        public async Task Invoke(HttpContext context, ICurrentUserAccessor currentUserAccessor) {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await AttachUserToContext(context, currentUserAccessor, token);

            await Next(context);
        }

        private async Task AttachUserToContext(HttpContext context, ICurrentUserAccessor currentUserAccessor, string token) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                Logger.LogInformation($"Token: {token}");
                foreach (var claim in jwtToken.Claims) {
                    Logger.LogInformation($"{claim.Type} - {claim.Value} ");
                }
                var userId = jwtToken.Claims.First(x => x.Type == "unique_name").Value;
                Logger.LogInformation($"Current user: {userId}");

                // attach user to context on successful jwt validation
                context.Items["User"] = await currentUserAccessor.FindById(userId);
            }
            catch (Exception e) {
                Logger.LogWarning($"Error: no user found: {e.Message}");
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        /// <summary>
        ///  Create the jwt token for the user that wee validate above
        /// </summary>
        /// <param name="user">User of the jwt token</param>
        /// <returns>jwt written to string</returns>
        public static string GenerateJwtToken(IUser user, ILogger logger) {
            //https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
            // generate token that is valid for 7 days
            logger.LogDebug($"Creating token for user id {user.Id}");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { new Claim("unique_name", user.Id) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
