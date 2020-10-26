using System.Threading.Tasks;

namespace Core.Interfaces {
  public interface ICurrentUserAccessor {
    Task<IUser> CurrentUser();
  }
}
