using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Interfaces;

using Infrastructure.Data;

using Microsoft.AspNetCore.Http;

namespace App {
    public class CurrentUserAccessor : ICurrentUserAccessor {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly ApplicationDbContext Context;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context) {
            HttpContextAccessor = httpContextAccessor;
            Context = context;
        }

        public async Task<IUser> CurrentUserByUsername(string username) {
            return await Context.Users.Where(user => user.UserName == username).FirstAsync();
        }

        public async Task<IUser> CurrentUser() {
            var userId = HttpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return await Context.Users.FindAsync(userId);
        }

        public string GetCurrentUsername() {
            return HttpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
