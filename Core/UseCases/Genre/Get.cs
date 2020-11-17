using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Core;
using Core.Interfaces;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

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
            public Handler(IGenreRepository repository, IMemoryCache cache, ILogger<Handler> logger) {
                Repository = repository;
                Cache = cache;
                Logger = logger;
            }

            public IGenreRepository Repository { get; }
            public IMemoryCache Cache { get; }
            public ILogger<Handler> Logger { get; }

            readonly string CacheKey = "ListOfGenres";

            public async Task<IEnumerable<string>> Handle(Command request, CancellationToken cancellationToken) {
                if(Cache.Get(CacheKey) is List<string> cachedResult) {
                    Logger.LogInformation("Using cached genre result");
                    return cachedResult;
                }

                var result = await Repository
                    .ItemsNoTracking()
                    .Select(genre => genre.Name)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                Cache.Set(CacheKey, result, DateTime.Now.AddDays(1));
                Logger.LogInformation("Re-setting genre cache");

                return result.AsEnumerable();
            }
        }
    }
}
