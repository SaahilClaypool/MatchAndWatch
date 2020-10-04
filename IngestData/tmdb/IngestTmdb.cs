using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Models.Title;

using Infrastructure.Data;
using Infrastructure;

using Shared;

using Tmdb.Api;
using Microsoft.EntityFrameworkCore;

namespace IngestData.Tmdb {
  class IngestTmdb {
    private readonly MoviePopulator Populator;
    private ApplicationDbContext Context;
    const int BATCH_SIZE = 100;
    public IngestTmdb(MoviePopulator populator) {
      Populator = populator;
      Context = CreateDbContext();
    }

    public async Task Ingest() {
      Context.Database.ExecuteSqlRaw($"DELETE FROM Genre;");
      Context.Database.ExecuteSqlRaw($"DELETE FROM Titles;");
      await Context.SaveChangesAsync();
      var titleTasks = Populator.GetTitles();
      List<Task<Title>> batch = new();
      // foreach (var (titleTask, index) in titleTasks.Take(10).WithIndex()) {
      foreach (var (titleTask, index) in titleTasks.WithIndex()) {
        batch.Add(titleTask);

        if ((index + 1) % BATCH_SIZE == 0) {
          await AddBatchToDatabase(batch);
          batch.Clear();
        }
      }
      await AddBatchToDatabase(batch);
    }

    private async Task AddBatchToDatabase(List<Task<Title>> batch) {
      var finishedTitleTasks = await ResolveTitles(batch);
      foreach (var title in finishedTitleTasks) {
        Context.Titles.Add(title);
      }
      await Context.SaveChangesAsync();
      await Context.DisposeAsync();
      Context = CreateDbContext();
    }

    private static ApplicationDbContext CreateDbContext() =>
       DbContextFactory.CreateDbContextWithOptions(new() { Log = false });

    private static async Task<IEnumerable<Title>> ResolveTitles(IEnumerable<Task<Title>> titles) {
      List<Title> resolvedTitles = new();
      foreach (var titleTask in titles) {
        try {
          var resolvedTitle = await titleTask;
          resolvedTitles.Add(resolvedTitle);
        }
        catch (Exception ex) {
          Console.WriteLine(ex.Message);
        }
      }

      return resolvedTitles;
    }
  }
}
