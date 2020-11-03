using System.Linq;

using Core.Interfaces;
using Core.Models;
using Core.Models.Title;

using Infrastructure.Data;

namespace Infrastructure.Repositories {
    public class TitleRepository : BaseRepository<Title>, ITitleRepository {
        public TitleRepository(ApplicationDbContext context) : base(context) {

        }

        public override IQueryable<Title> Items() => Context.Titles.AsQueryable();

        public IQueryable<Genre> TitleGenres() => Context.Set<Genre>().AsQueryable();
    }
}
