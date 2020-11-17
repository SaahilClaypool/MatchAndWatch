using System.Linq;
using System.Threading.Tasks;

using Core.Models;
using  Core.Models.Title;

namespace Core.Interfaces {
    public interface ISessionRepository : IBaseRepository<Session> {
        IQueryable<Rating> Ratings();
        void AddRating(Rating rating);
    }
}
