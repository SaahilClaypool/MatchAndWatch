using System.Threading.Tasks;

namespace Core.Interfaces {
    public interface ICurrentUserAccessor {
        Task<IUser> CurrentUser();
        Task<IUser> FindByUsername(string username);
        Task<IUser> FindById(string id);
    }
}
