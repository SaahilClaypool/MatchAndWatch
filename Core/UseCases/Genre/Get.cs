using MediatR;
using System;
using System.Collections.Generic;
using Core;
using System.Threading.Tasks;
using System.Threading;
using Core.Interfaces;
using System.Linq;

namespace Core.UseCases.Genre {

  public class Get {
    public record Command : IRequest<IEnumerable<Models.Title.Genre>> { };
    public class Handler : IRequestHandler<Command, IEnumerable<Models.Title.Genre>> {
      public Handler(IGenreRepository repository) {
        Repository = repository;
      }

      public IGenreRepository Repository { get; }

      public Task<IEnumerable<Models.Title.Genre>> Handle(Command request, CancellationToken cancellationToken) =>
        Task.FromResult(Repository.Items().AsEnumerable());
    }
  }

  public class GetUniqueGenreNames {
    public record Command : IRequest<IEnumerable<string>> { };
    public class Handler : IRequestHandler<Command, IEnumerable<string>> {
      public Handler(IGenreRepository repository) {
        Repository = repository;
      }

      public IGenreRepository Repository { get; }

      public Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken) =>
        Task.FromResult(
          Repository.ItemsNoTracking().Select(genre => genre.Name).Distinct().AsEnumerable()
          );
    }
  }
}
