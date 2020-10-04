using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shared;

namespace tmdb.StaticData {
  public class MovieReader {
    public string File { get; set; }

    public MovieReader(string file) {
      File = file;
    }

    public IEnumerable<StaticMovieRecord> GetRecords() {
      using var file = new System.IO.StreamReader(File);
      string line;
      while ((line = file.ReadLine()) != null) {
        var record = JsonSerializer.Deserialize<StaticMovieRecord>(line);
        yield return record;
      }
    }
  }

  public class StaticMovieRecord {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("original_title")]
    public string OriginalTitle { get; set; }
  }
}
