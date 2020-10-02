
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
    protected int BatchSize { get; set; } = 100000;
    abstract public Task Ingest();
    public Func<ApplicationDbContext> CreateContext { get; set; }

    protected async Task IngestRecords<T>(IEnumerable<T> records, Func<ApplicationDbContext, DbSet<T>> DbSet) where T : class {
      List<T> pendingRecords = new();
      foreach (var record in records.Select((val, i) => new { Value = val, Index = i })) {
        if (MaxRecords != 0 && record.Index > MaxRecords) {
          break;
        }

        pendingRecords.Add(record.Value);

        if (record.Index % BatchSize == 0) {
          await DbSet(Context).AddRangeAsync(pendingRecords);
          System.Console.WriteLine("Inserting");
          await Context.SaveChangesAsync();
          System.Console.WriteLine("Done Inserting");
          await Context.DisposeAsync();
          pendingRecords = new();
          Context = CreateContext();
        }
      }
      await DbSet(Context).AddRangeAsync(pendingRecords);
      System.Console.WriteLine("Inserting");
      await Context.SaveChangesAsync();
    }
    public async Task Clear(string table) {
      using var context = CreateContext();
      context.Database.ExecuteSqlRaw($"DELETE FROM {table};");
      await context.SaveChangesAsync();
    }

    public Task<bool> IsEmpty<T>(Func<ApplicationDbContext, DbSet<T>> dbset) where T : class {
      using var context = CreateContext();
      return dbset(context).AnyAsync();
    }
  }

}
