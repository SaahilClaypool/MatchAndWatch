using System.Linq;

using Core.Interfaces;
using Core.Models;

using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories {
    public class SessionRepository : BaseRepository<Session>, ISessionRepository {
        public SessionRepository(ApplicationDbContext context) : base(context) {

        }

        public override IQueryable<Session> Items() => Context.Sessions.AsQueryable();

        public IQueryable<Rating> Ratings() => Context.Set<Rating>().AsNoTracking().AsQueryable();
    }
}
