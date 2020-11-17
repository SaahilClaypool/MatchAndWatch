using System.Threading.Tasks;

using Tmdb.Api;

namespace Tmdb.Services {
    public class MovieMeta {
        public MovieClient Client { get; }
        private readonly string PosterBase = "https://image.tmdb.org/t/p/w500";
        private readonly string GenericImage = "https://image.tmdb.org/t/p/w500/wwemzKWzjKYJFfCeiB57q3r4Bcm.png";

        public MovieMeta(MovieClient client) {
            Client = client;
        }

        public async Task<string> PosterPath(string titleId) {
            var details = await Client.Details(titleId);

            return details.PosterPath switch {
                string poster => $"{PosterBase}/{poster}",
                _ => GenericImage
            };
        }
    }
}
