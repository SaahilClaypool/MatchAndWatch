using System.Linq;
using System.Threading.Tasks;

using Core.Models.Title;

namespace Core.Interfaces {
  public interface ITitleRepository {
    public IQueryable<TitleAgg> Titles(bool withRatings = false);
    public void Add(TitleAgg title);
    public void Remove(TitleAgg title);
    public Task Save();
  }
}
