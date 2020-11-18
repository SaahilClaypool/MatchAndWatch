using System.Collections.Generic;
using System.Linq;

namespace Core.Interfaces {
    public interface IUserRepository : IBaseRepository<IUser> {
        IQueryable<IUser> Friends();
    }
}
