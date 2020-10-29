using System.Collections.Generic;

using Core.Interfaces;

namespace Core.Models {
  public class Session : Entity {
    public IEnumerable<string> Genres { get; init; } = new List<string>();
    public IUser Creater { get; init; }
    public IEnumerable<ParticipantStatus> Participants { get; init; }
  }

  public class ParticipantStatus : Entity {
    public enum State {
      Invited,
      Completed
    }
    public IUser User { get; init; }
    public State CurrentState { get; init; }
  }
}
