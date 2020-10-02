using tmdb.StaticData;
using System;
using System.Threading.Tasks;
using Tmdb.Api;
using System.Linq;
using Shared;

namespace IngestData {
  class Program {
    static async Task Main(string[] _args) {
      var client = TmdbClient.Default();
      var populator = new MoviePopulator("./data/movie_ids_10_01_2020.json", client);
      var titles = populator.GetTitles();
      foreach (var titleTask in titles.Take(10)) {
        var title = await titleTask;
        System.Console.WriteLine(title.ToJson());
      }
    }
  }
}
