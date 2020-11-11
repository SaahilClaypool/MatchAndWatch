using System.Linq;
using System.Threading.Tasks;

using Core.Models;

namespace Core.Interfaces {
    public interface ISessionRepository : IBaseRepository<Session> { 
        IQueryable<Rating> Ratings();
    }
}
