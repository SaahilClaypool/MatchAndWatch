using System.Linq;
using System.Threading.Tasks;

using Core.Models.Title;

namespace Core.Interfaces {
  public interface ITitleRepository {
    public IQueryable<Title> Titles();
    public void Add(Title title);
    public void Remove(Title title);
    public Task Save();
  }
}
