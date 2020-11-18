using System;
using System.Collections.Generic;
using System.Linq;

using Core.Interfaces;

namespace Core.Models {
    public class Session : Entity {
        public IEnumerable<string> Genres { get; set; } = new List<string>();
        public IUser Creater { get; set; }
        public IEnumerable<ParticipantStatus> Participants { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
        public string Name { get; set; }
        public InviteLink Invite { get; set; } = null!;
    }

    public class ParticipantStatus : Entity {
        public enum State {
            Invited,
            Completed
        }
        public IUser User { get; set; }
        public State CurrentState { get; set; }
    }

    public class Rating : Entity {
        public enum ScoreType {
            UP,
            DOWN,
            SUPER,
            UNDECIDED
        }
        public string UserId { get; set; }
        public IUser User { get; set; }
        public string SessionId { get; set; }
        public Session Session { get; set; }
        public string TitleId { get; set; }
        public Title.Title Title { get; set; }
        public ScoreType Score { get; set; }
    }

    public class InviteLink : Entity {
        public Session Session { get; set; } = null!;
        public string SessionId { get; set; } = null!;
        public string Code { get; set; } = default!;
        public DateTime? Expiration { get; set; } = null!;
    }
}
