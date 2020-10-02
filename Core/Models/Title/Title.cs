using System.Collections.Generic;
/// https://www.imdb.com/interfaces/
namespace Core.Models.Title {
  /// Pull from TitleBasics
  public class Title {
    public string Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public int? ReleaseYear { get; set; }
    public int? EndYear { get; set; }
    public int RunTime { get; set; }
    public ICollection<Genre> Genres { get; set; }
    public float Popularity { get; set; }
    public float RatingAverage { get; set; }
    public int RatingCount { get; set; }
    public string ImdbId { get; set; }
  }
}
