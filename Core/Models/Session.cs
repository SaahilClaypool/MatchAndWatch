using System.Collections.Generic;
using System.Linq;

using Core.Interfaces;

namespace Core.Models {
    public class Session : Entity {
        public IEnumerable<string> Genres { get; set; } = new List<string>();
        public IUser Creater { get; set; }
        public IEnumerable<ParticipantStatus> Participants { get; set; }
        public IQueryable<Rating> Ratings { get; set; }
        public string Name { get; set; }
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
}
