using System;

using Infrastructure;
using System.Linq;

using IngestData.imdb;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

namespace IngestData {
    class Program {
        static void Main(string[] _args) {
            Console.WriteLine("Hello World!");
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
                return new DbContextFactory().CreateDbContextWithOptions(new() { Log = false });
            };

            ImdbIngester ingester = new(paths) {
                CreateContext = createContext
            };
            ingester.MaxRecords = 0;
            ingester.Ingest().Wait();

        }
    }
}
