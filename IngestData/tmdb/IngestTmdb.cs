using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Models.Title;

using Infrastructure;
using Extensions;
using Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

using Tmdb.Api;

namespace IngestData.Tmdb {
    class IngestTmdb {
        private readonly MoviePopulator Populator;
        private ApplicationDbContext Context;
        const int BATCH_SIZE = 500;
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
            var finishedTitleTasks = await Task.WhenAll(batch.Select(title => ResolveTitle(title)));
            foreach (var title in finishedTitleTasks) {
                if (title is not null) {
                    Context.Titles.Add(title);
                }
            }
            await Context.SaveChangesAsync();
            await Context.DisposeAsync();
            Context = CreateDbContext();
        }

        private static ApplicationDbContext CreateDbContext() =>
           DbContextFactory.CreateDbContextWithOptions(new() { Log = false });

        private static async Task<Title> ResolveTitle(Task<Title> titleTask) {
            try {
                var resolvedTitle = await titleTask;
                return resolvedTitle;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
