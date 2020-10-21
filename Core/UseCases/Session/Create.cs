using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.Models.Title;

using FluentValidation;

namespace Core.UseCases.Session {
  public class Create {
    public record Command(
      IEnumerable<string> Genres
    );

    public class CommandValidator : AbstractValidator<Command> {
      public CommandValidator() {
        RuleFor(c => c.Genres).NotNull().NotEmpty();
      }
    }

    public class Handler {
      private ITitleRepository TitleRepository { get; init; }
      private ICurrentUserAccessor CurrentUserAccessor { get; init; }

      public Handler(ITitleRepository titleRepository, ICurrentUserAccessor currentUserAccessor) {
        TitleRepository = titleRepository;
        CurrentUserAccessor = currentUserAccessor;
      }

      public async Task Handle(Command message) {
      }
    }
  }
}
