using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Core.UseCases.Session {
    public class ParticipatedByCurrentUser {
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
                var items = SessionRepository.Items()
                    .Include(session => session.Participants)
                    .Include(session => session.Creater)
                    .Where(item => item.Participants.Any(particpant => particpant.User.Id == user.Id))
                    ;
                return items;
            }
        }
    }
}
