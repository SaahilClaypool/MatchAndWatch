using System.Threading.Tasks;
using System.Collections.Generic;

using Core.Models.Title;
using tmdb.StaticData;
using Shared;

namespace Tmdb.Api {
  public class MoviePopulator {
    private MovieClient Client;
    private string Path;
    public MoviePopulator(string path, TmdbClient client) {
      Path = path;
      Client = new MovieClient(client);
    }

    public IEnumerable<Task<Title>> GetTitles() {
      var reader = new MovieReader(Path);

      foreach (var record in reader.GetRecords()) {
        yield return Client.Details(record.Id.ToString());
      }
    }
  }
}
