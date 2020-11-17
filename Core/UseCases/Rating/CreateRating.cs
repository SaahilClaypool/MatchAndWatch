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
using static Core.Models.Rating;

namespace Core.UseCases.Rating {
    public class CreateRating {
        public record Command(
            string SessionId,
            string MovieId,
            ScoreType Score
        ) : IRequest<Response>;

        public record Response();

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator(ISessionRepository sessionRepository, ITitleRepository titleRepository) {
                RuleFor(c => c.SessionId).NotNull().NotEmpty();
                RuleFor(c => c.SessionId).NotNull().NotEmpty().MustAsync(async (id, token) => {
                    return await sessionRepository
                                .ItemsNoTracking()
                                .Where(session => session.Id == id).AnyAsync(token);
                });
                RuleFor(c => c.MovieId).NotNull().NotEmpty();
                RuleFor(c => c.MovieId).NotNull().NotEmpty().MustAsync(async (id, token) => {
                    return await titleRepository
                                .ItemsNoTracking()
                                .Where(session => session.Id == id).AnyAsync(token);
                });
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
                    // ignore undecided ones for now (they could pop again later...)
                    if(request.Score == ScoreType.UNDECIDED) {
                        return new();
                    }

                    var user = await CurrentUserAccessor.CurrentUser();
                    var rating = new Models.Rating() {
                        User = user,
                        TitleId = request.MovieId,
                        SessionId = request.SessionId,
                        Score = request.Score
                    };
                    SessionRepository.AddRating(rating);
                    await SessionRepository.Save();
                    return new();
                }
            }
        }
    }
}
