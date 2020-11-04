using System.Threading.Tasks;

namespace Core.Interfaces {
    public interface ICurrentUserAccessor {
        Task<IUser> CurrentUser();
        Task<IUser> CurrentUserByUsername(string username);
    }
}
