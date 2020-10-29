using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using Core.Interfaces;
using Core.Models;
using MediatR;

using FluentValidation;

namespace Core.UseCases.Session {
  public class GetAllForCurrentUser {
    public record Command : IRequest<IEnumerable<Models.Session>> { }

    public class Handler : IRequestHandler<Command, IEnumerable<Models.Session>> {
      private ISessionRepository SessionRepository { get; init; }
      private ICurrentUserAccessor CurrentUserAccessor { get; init; }

      public Handler(ISessionRepository sessionRepository, ICurrentUserAccessor currentUserAccessor) {
        SessionRepository = sessionRepository;
        CurrentUserAccessor = currentUserAccessor;
      }

      public async Task<IEnumerable<Models.Session>> Handle(Command request, CancellationToken cancellationToken) {
        var user = await CurrentUserAccessor.CurrentUser();
        var items = SessionRepository.Items().Where(item => item.Creater.Id == user.Id);
        return items;
      }
    }
  }
}
