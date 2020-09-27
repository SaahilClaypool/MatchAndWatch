
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace IngestData.imdb {
    public abstract class AIngester {
        protected ApplicationDbContext _Context;
        protected ApplicationDbContext Context { get => _Context ??= CreateContext(); set => _Context = value; }
        public int MaxRecords { get; set; }
        protected int BatchSize { get; set; } = 10000;
        abstract public Task Ingest();
        public Func<ApplicationDbContext> CreateContext { get; set; }

        protected async Task IngestRecords<T>(IEnumerable<T> records, DbSet<T> DbSet) where T : class {
            List<T> pendingRecords = new();
            foreach (var record in records.Select((val, i) => new { Value = val, Index = i })) {
                if (MaxRecords != 0 && record.Index > MaxRecords) {
                    break;
                }

                pendingRecords.Add(record.Value);

                if (record.Index % BatchSize == 0) {
                    await DbSet.AddRangeAsync(pendingRecords);
                    await Context.SaveChangesAsync();
                    pendingRecords = new();
                }
            }
        }
    }

}
