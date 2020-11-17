using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;
using Core.Services.Session;

using FluentValidation;

using Extensions;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core.UseCases.Rating {
    public class NewRating {
        public record Command(
            string SessionId
        ) : IRequest<Response>;

        public record Response(
          string MovieTitle,
          string MovieId,
          int? Year,
          int? RunTime,
          IEnumerable<string> Genres
        );

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator(ISessionRepository sessionRepository) {
                RuleFor(c => c.SessionId).NotNull().NotEmpty();
                RuleFor(c => c.SessionId).NotNull().NotEmpty().MustAsync(async (id, token) => {
                    return await sessionRepository
                                .ItemsNoTracking()
                                .Where(session => session.Id == id).AnyAsync(token);
                });
            }
        }

        public class Handler : IRequestHandler<Command, Response> {
            private ISessionRepository SessionRepository { get; }
            public ITitleRepository TitleRepository { get; }
            public ICurrentUserAccessor CurrentUserAccessor { get; }
            public ILogger<Handler> Logger { get; }

            public Handler(
                ISessionRepository sessionRepository,
                ITitleRepository titleRepository,
                ICurrentUserAccessor currentUserAccessor,
                ILogger<Handler> logger) {
                SessionRepository = sessionRepository;
                TitleRepository = titleRepository;
                CurrentUserAccessor = currentUserAccessor;
                Logger = logger;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken) {
                var session = await SessionRepository
                    .Items()
                    .Include(session => session.Ratings)
                    .Where(session => session.Id == request.SessionId)
                    .FirstAsync(cancellationToken);
                var currentUser = await CurrentUserAccessor.CurrentUser();
                var manager = new SessionManager(SessionRepository, TitleRepository, session, currentUser.Id, Logger);

                var title = await manager.NextTitle();
                Logger.LogDebug(title.ToJson());
                return new(
                    title.Name,
                    title.Id!,
                    title.ReleaseYear,
                    title.RunTime,
                    // https://docs.microsoft.com/en-us/ef/core/querying/related-data
                    // Need to specify that we want generes to be loaded
                    new List<string>() // TODO include this originally
                );
            }
        }
    }
}
