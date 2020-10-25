using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;
using MediatR;

using FluentValidation;

namespace Core.UseCases.Session {
  public class Create {
    public record Command(
      IEnumerable<string> Genres
    ) : IRequest;

    public class CommandValidator : AbstractValidator<Command> {
      public CommandValidator() {
        RuleFor(c => c.Genres).NotNull().NotEmpty();
      }
    }

    public class Handler : IRequestHandler<Command> {
      private ISessionRepository SessionRepository { get; init; }
      private ICurrentUserAccessor CurrentUserAccessor { get; init; }

      public Handler(ISessionRepository sessionRepository, ICurrentUserAccessor currentUserAccessor) {
        SessionRepository = sessionRepository;
        CurrentUserAccessor = currentUserAccessor;
      }

      public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {
        var user = CurrentUserAccessor.CurrentUser();
        Models.Session session = new() {
          Creater = user,
          Genres = request.Genres,
          Participants = new List<ParticipantStatus>() {
            new() { User = user, CurrentState = ParticipantStatus.State.Invited }
          }
        };
        SessionRepository.Add(session);
        await SessionRepository.Save();
        return new();
      }
    }
  }
}
