using System;
using System.Linq;
using System.Threading.Tasks;

using Infrastructure;

using IngestData.Tmdb;

using tmdb.StaticData;

using Tmdb.Api;

namespace IngestData {
    class Program {
        static async Task Main(string[] _args) {
            var client = TmdbClient.Default();
            var populator = new MoviePopulator("./data/movie_ids_10_01_2020.json", client);
            var ingester = new IngestTmdb(populator);
            await ingester.Ingest();
        }
    }
}
