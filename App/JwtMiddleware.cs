using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Core.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace App {
    public class JwtMiddleware {
        private readonly RequestDelegate Next;

        public JwtMiddleware(RequestDelegate next) {
            Next = next;
        }

        public async Task Invoke(HttpContext context, ICurrentUserAccessor currentUserAccessor) {
            System.Console.WriteLine("SMC: Critical");
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            System.Console.WriteLine(token);

            if (token != null)
                await AttachUserToContext(context, currentUserAccessor, token);

            System.Console.WriteLine("DONE");
            await Next(context);
        }

        private static async Task AttachUserToContext(HttpContext context, ICurrentUserAccessor currentUserAccessor, string token) {
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("The quick brown foxes jumped over the lazy brown dogs");
                tokenHandler.ValidateToken(token, new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

                // attach user to context on successful jwt validation
                context.Items["User"] = await currentUserAccessor.FindById(userId);
            }
            catch {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }

        /// <summary>
        ///  Create the jwt token for the user that wee validate above
        /// </summary>
        /// <param name="user">User of the jwt token</param>
        /// <returns>jwt written to string</returns>
        public static string GenerateJwtToken(IUser user) {
            //https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("The quick brown foxes jumped over the lazy brown dogs");
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
