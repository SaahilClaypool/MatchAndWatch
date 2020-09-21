using System;

using IngestData.imdb;

namespace IngestData {
    class Program {
        static void Main(string[] _args) {
            Console.WriteLine("Hello World!");
            ImdbIngester ingester = new(new() {
                NameBasics = "./data/name.basics.tsv",
                TitleBasics = "./data/title.basics.tsv",
                TitleCrew = "./data/title.crew.tsv",
                TitleEpisode = "./data/title.episodes.tsv",
                TitlePrincipals = "./data/title.principals.tsv",
                TitleRatings = "./data/title.ratings.tsv",
            });
            ingester.Ingest();

        }
    }
}
