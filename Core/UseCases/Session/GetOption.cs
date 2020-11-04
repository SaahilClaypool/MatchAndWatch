using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;


using FluentValidation;

using MediatR;
using System;

namespace Core.UseCases.Session {
    public class GetOption {
        public record Command(
            string Id
        ) : IRequest;

        public class CommandValidator : AbstractValidator<Command> {
            public CommandValidator(ISessionRepository sessionRepository) {
                RuleFor(c => c.Id).NotNull().NotEmpty().MustAsync(async (id, token) => {
                    return await sessionRepository.ItemsNoTracking().Where(session => session.Id == id).AnyAsync(token);
                });
            }
        }

        public class Handler : IRequestHandler<Command> {
            private ISessionRepository SessionRepository { get; init; }

            public Handler(ISessionRepository sessionRepository) {
                SessionRepository = sessionRepository;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken) {
                var session = SessionRepository.ItemsNoTracking().Where(session => session.Id == request.Id);
                throw new NotImplementedException();
            }
        }
    }
}
