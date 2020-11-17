using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Models.Title;

using Extensions;

using tmdb.StaticData;

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
