using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models.Title;
using Core.Models;

using FluentValidation;

namespace Core.UseCases.Session {
  public class Create {
    public record Command(
      IEnumerable<string> Genres
    );

    public class CommandValidator : AbstractValidator<Command> {
      public CommandValidator() {
        RuleFor(c => c.Genres).NotNull().NotEmpty();
      }
    }

    public class Handler {
      private ISessionRepository SessionRepository { get; init; }
      private ICurrentUserAccessor CurrentUserAccessor { get; init; }

      public Handler(ISessionRepository sessionRepository, ICurrentUserAccessor currentUserAccessor) {
        SessionRepository = sessionRepository;
        CurrentUserAccessor = currentUserAccessor;
      }

      public async Task Handle(Command message) {
        var user = CurrentUserAccessor.CurrentUser();
        Models.Session session = new() {
          Creater = user,
          Genres = message.Genres,
          Participants = new List<ParticipantStatus>() {
            new() { User = user, CurrentState = ParticipantStatus.State.Invited }
          }
        };
        SessionRepository.Add(session);
        await SessionRepository.Save();
      }
    }
  }
}
