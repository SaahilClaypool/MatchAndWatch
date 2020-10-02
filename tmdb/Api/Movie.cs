using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Core.Models.Title;

using Shared;

namespace Tmdb.Api {
  public class MovieClient {
    private TmdbClient Client { get; }
    public MovieClient(TmdbClient client) {
      Client = client;
    }

    public async Task<Title> Details(string id) {
      var path = $"movie/{id}";
      path.Dbg();
      var details = await Client.MakeRequest<MovieDetails>(path);
      details.Dbg();
      return details.ToTitle();
    }
  }

  public class MovieDetails {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("imdb_id")]
    public string ImdbId { get; set; }

    [JsonPropertyName("genres")]
    public IEnumerable<GenreDetails> Genres { get; set; }

    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; }

    public Title ToTitle() {
      return new Title {
        Id = Id.ToString(),
        Genres = Genres.Select(genre => genre.ToGenre()).ToList(),
        Name = OriginalTitle,
      };
    }
  }

  public class GenreDetails {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    public Genre ToGenre() {
      return new();
    }
  }
}
