using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;
using Core.Services.Session;

using FluentValidation;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace Core.UseCases.Invite {
    public class GetToken {
        public record Command(
            string Token
        ) : IRequest<string>;

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator(ISessionRepository sessionRepository) {
                RuleFor(c => c.Token).NotNull().NotEmpty().MustAsync(async (token, cancellationToken) => {
                    return await sessionRepository
                        .ItemsNoTracking()
                        .Select(session => session.Invite)
                        .Where(invite => invite.Code == token)
                        .AnyAsync(cancellationToken);
                });
            }
        }

        public class Handler : IRequestHandler<Command, string> {
            private ISessionRepository SessionRepository { get; init; }
            public ICurrentUserAccessor CurrentUserAccessor { get; }

            public Handler(ISessionRepository sessionRepository, ICurrentUserAccessor currentUserAccessor) {
                SessionRepository = sessionRepository;
                CurrentUserAccessor = currentUserAccessor;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken) {
                var session = await SessionRepository
                                            .Items()
                                            .Include(session => session.Invite)
                                            .Include(session => session.Participants)
                                            .Where(session => session.Invite.Code == request.Token)
                                            .FirstAsync(cancellationToken);
                var user = await CurrentUserAccessor.CurrentUser();
                var status = new ParticipantStatus() {
                    User = user,
                    CurrentState = ParticipantStatus.State.Completed
                };
                session.Participants = session.Participants.Append(status);
                await SessionRepository.Save();
                return session.Id!;
            }
        }
    }
}
