using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;

using Infrastructure.Data;

namespace Infrastructure.Repositories {
  public abstract class BaseRepository<T> : IBaseRepository<T> {
    protected ApplicationDbContext Context { get; set; }
    public BaseRepository(ApplicationDbContext context) {
      Context = context;
    }

    public abstract IQueryable<T> Items();

    public virtual void Add(T item) {
      Context.Add(item);
    }

    public virtual void Remove(T item) {
      Context.Remove(item);
    }

    public virtual Task Save() {
      return Context.SaveChangesAsync();
    }

    public virtual Task Save(CancellationToken token) {
      return Context.SaveChangesAsync(token);
    }
  }
}
