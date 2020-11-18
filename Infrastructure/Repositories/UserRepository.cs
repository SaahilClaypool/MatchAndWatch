using System.Linq;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;

using Infrastructure.Data;
using Infrastructure.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
    public class UserRepository : IUserRepository {
        public UserRepository(ApplicationDbContext context) {
            Context = context;
        }

        public ApplicationDbContext Context { get; }

        public void Add(IUser item) => Context.Add((ApplicationUser)item);

        public IQueryable<IUser> Items() => Context.Users.AsQueryable();

        public IQueryable<IUser> ItemsNoTracking() => Items().AsNoTracking();

        public void Remove(IUser item) => Context.Remove((ApplicationUser)item);

        public Task Save() => Context.SaveChangesAsync();

        public async Task<IUser> Find(string Id) => await Context.Users.Where(user => user.Id == Id).FirstAsync();

        public IQueryable<IUser> Friends() => throw new System.NotImplementedException();
    }
}
