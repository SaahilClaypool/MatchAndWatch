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
    public class JoinSession {
        public record Command(
            string Id
        ) : IRequest<string>;

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator(ISessionRepository sessionRepository) {
                RuleFor(c => c.Id).NotNull().NotEmpty().MustAsync(async (id, token) => {
                    return await sessionRepository.ItemsNoTracking().Where(session => session.Id == id).AnyAsync(token);
                });
            }
        }

        public class Handler : IRequestHandler<Command, string> {
            private ISessionRepository SessionRepository { get; init; }

            public Handler(ISessionRepository sessionRepository) {
                SessionRepository = sessionRepository;
            }

            public async Task<string> Handle(Command request, CancellationToken cancellationToken) {
                var session = await SessionRepository
                    .Items()
                    .Where(session => session.Id == request.Id)
                    .Include(session => session.Invite)
                    .FirstAsync(cancellationToken)
                    ;
                var invite = session.Invite;

                if(invite.Expiration > DateTime.UtcNow) {
                    return invite.Code;
                }

                invite.Code = invite.GenerateToken();
                invite.Expiration = DateTime.UtcNow.AddDays(1);
                await SessionRepository.Save();
                return invite.Code;
            }
        }
    }
}
