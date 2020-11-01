using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;

using FluentValidation;

using MediatR;

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
