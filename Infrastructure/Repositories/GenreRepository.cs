using System.Linq;

using Core.Interfaces;
using Core.Models;
using Core.Models.Title;

using Infrastructure.Data;

namespace Infrastructure.Repositories {
  public class GenreRepository : BaseRepository<Genre>, IGenreRepository {
    public GenreRepository(ApplicationDbContext context) : base(context) {

    }

    public override IQueryable<Genre> Items() => Context.Genres.AsQueryable();
  }
}
