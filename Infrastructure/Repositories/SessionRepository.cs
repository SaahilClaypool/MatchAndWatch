using System.Linq;

using Core.Interfaces;
using Core.Models;

using Infrastructure.Data;

namespace Infrastructure.Repositories {
  public class SessionRepository : BaseRepository<Session>, ISessionRepository {
    public SessionRepository(ApplicationDbContext context) : base(context) {

    }

    public override IQueryable<Session> Items() => Context.Sessions.AsQueryable();
  }
}
