using System;
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
    public class GetSession {
        public record Command(
            string Id
        ) : IRequest<Models.Session> {
            public bool IncludeRatings { get; init; } = false;
        };

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator(ISessionRepository sessionRepository) {
                RuleFor(c => c.Id).NotNull().NotEmpty().MustAsync(async (id, token) => {
                    return await sessionRepository.ItemsNoTracking().Where(session => session.Id == id).AnyAsync(token);
                });
            }
        }

        public class Handler : IRequestHandler<Command, Models.Session> {
            private ISessionRepository SessionRepository { get; init; }

            public Handler(ISessionRepository sessionRepository) {
                SessionRepository = sessionRepository;
            }

            public async Task<Models.Session> Handle(Command request, CancellationToken cancellationToken) {
                var items = SessionRepository
                    .ItemsNoTracking()
                    .Where(session => session.Id == request.Id);
                if (request.IncludeRatings) {
                    items = items.Include(session => session.Ratings)
                                .ThenInclude(rating => rating.Title);
                    items = items.Include(session => session.Ratings)
                                .ThenInclude(rating => rating.User);
                }
                var session = await items.FirstAsync(cancellationToken);

                return session;
            }
        }
    }
}
