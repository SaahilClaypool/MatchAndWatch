using System;
using System.Linq;

using Infrastructure;
using Infrastructure.Data;

using IngestData.imdb;

using Microsoft.EntityFrameworkCore;

namespace IngestData.imdb {
    public class Main {
        public static void Ingest() {
            ImdbTsvPaths paths = new() {
                NameBasics = "./data/name.basics.tsv",
                TitleBasics = "./data/title.basics.tsv",
                TitleCrew = "./data/title.crew.tsv",
                TitleEpisode = "./data/title.episodes.tsv",
                TitlePrincipals = "./data/title.principals.tsv",
                TitleRatings = "./data/title.ratings.tsv",
            };

            int num = 0;
            ApplicationDbContext createContext() {
                System.Console.WriteLine($"Creating context {num++}");
                return DbContextFactory.CreateDbContextWithOptions(new() { Log = false });
            };

            ImdbIngester ingester = new(paths) {
                CreateContext = createContext
            };
            ingester.MaxRecords = 0;
            ingester.Ingest().Wait();
        }
    }
}
