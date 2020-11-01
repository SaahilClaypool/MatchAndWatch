using tmdb.StaticData;
using System;
using System.Threading.Tasks;
using Tmdb.Api;
using System.Linq;
using Infrastructure;
using IngestData.Tmdb;

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
