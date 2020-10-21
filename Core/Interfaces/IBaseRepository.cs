using System.Linq;
using System.Threading.Tasks;

using Core.Models.Title;

namespace Core.Interfaces {
  public interface IBaseRepository<T> {
    public IQueryable<T> Items();
    public void Add(T item);
    public void Remove(T item);
    public Task Save();
  }
}
