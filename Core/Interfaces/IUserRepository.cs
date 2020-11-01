using System.Collections.Generic;

namespace Core.Interfaces {
    public interface IUserRepository : IBaseRepository<IUser> {
        IEnumerable<IUser> Friends();
    }
}
