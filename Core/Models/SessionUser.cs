using Core.Interfaces;

namespace Core.Models {

    public class SessionUser {
        public int UserId { get; set; }
        public IUser User { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }

        public bool IsHost { get; set; }
    }
}
