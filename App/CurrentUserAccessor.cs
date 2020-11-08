using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Interfaces;

using Infrastructure.Data;

using Microsoft.AspNetCore.Http;
using Infrastructure;

namespace App {
    public class CurrentUserAccessor : ICurrentUserAccessor {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly ApplicationDbContext Context;

        public CurrentUserAccessor(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context) {
            HttpContextAccessor = httpContextAccessor;
            Context = context;
        }

        public async Task<IUser> FindByUsername(string username) {
            return await Context.Users.Where(user => user.UserName == username).FirstAsync();
        }

        public async Task<IUser> FindById(string id) {
            return await Context.Users.Where(user => user.Id == id).FirstAsync();
        }

        public Task<IUser> CurrentUser() => Task.FromResult((IUser)HttpContextAccessor.HttpContext.Items["User"]);

        public string GetCurrentUsername() {
            return HttpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "unique_name")?.Value;
        }
    }
}
