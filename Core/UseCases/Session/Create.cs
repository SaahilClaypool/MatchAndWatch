using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models;
using Core.Services.Session;

using FluentValidation;

using MediatR;

namespace Core.UseCases.Session {
    public class Create {
        public record Command(
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

            public Handler(ISessionRepository sessionRepository, ICurrentUserAccessor currentUserAccessor) {
                SessionRepository = sessionRepository;
                CurrentUserAccessor = currentUserAccessor;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken) {
                var user = await CurrentUserAccessor.CurrentUser();
                Models.Session session = new() {
                    Creater = user,
                    Genres = request.Genres,
                    Name = request.Name,
                    Participants = new List<ParticipantStatus>() {
                        new() {
                            User = user,
                            CurrentState = ParticipantStatus.State.Completed
                        }
                    },
                    Invite = new() { 
                        Expiration = DateTime.UtcNow.AddDays(1),
                        Code = InviteService.GenerateToken(null!)
                    }
                };
                SessionRepository.Add(session);
                await SessionRepository.Save();
                return new(session.Id!);
            }
        }
    }
}
