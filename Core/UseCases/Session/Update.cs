using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.UseCases.Session {
    public class Update {
        public record Command(
            string Id,
            IEnumerable<string> Genres,
            string Name
        ) : IRequest<Response>;

        public record Response(
          string Id
        );

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator() {
                RuleFor(c => c.Genres).NotNull().NotEmpty();
                RuleFor(c => c.Name).NotNull().NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Response> {
            private ISessionRepository SessionRepository { get; init; }
            private ICurrentUserAccessor CurrentUserAccessor { get; init; }
            public ILogger<Handler> Logger { get; }

            public Handler(ISessionRepository sessionRepository, ICurrentUserAccessor currentUserAccessor, ILogger<Handler> logger) {
                SessionRepository = sessionRepository;
                CurrentUserAccessor = currentUserAccessor;
                Logger = logger;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken) {
                var user = await CurrentUserAccessor.CurrentUser();
                var session = await SessionRepository.Items().Where(session => session.Id == request.Id).FirstAsync(cancellationToken);
                session.Genres = request.Genres;
                session.Name = request.Name;
                await SessionRepository.Save();

                Logger.LogDebug($"saved session with {session.Genres.Count()} genres\n");

                return new(session.Id);
            }
        }
    }
}
