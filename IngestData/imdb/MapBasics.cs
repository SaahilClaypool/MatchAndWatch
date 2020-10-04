using System.Linq;

using Core.Models.Title;

using Shared;

namespace IngestData.imdb {
  class MapBasics : AMapper<BasicsRow, Title> {
    public override Title Map(BasicsRow from) {
      Title title = new() {
        Id = from.Tconst,
        Type = from.TitleType,
        Name = from.PrimaryTitle,
        ReleaseYear = ParseYear(from.StartYear),
        EndYear = ParseYear(from.EndYear),
        RunTime = ParseYear(from.RuntimeMinutes) ?? 0,
      };

      title.Genres = from.Genres.Split(";").Select(genre => new Genre() {
        Name = genre,
        Title = title
      }).ToList();

      return title;
    }

    static int? ParseYear(string year) {
      try {
        return year.Equals("\\N") ? null : int.Parse(year);
      }
      catch {
        return null;
      }
    }
  }

  public class BasicsRow {
    public string Tconst { get; set; } // id
    public string TitleType { get; set; }
    public string PrimaryTitle { get; set; }
    public string OriginalTitle { get; set; }
    public string IsAdult { get; set; }
    public string StartYear { get; set; }
    public string EndYear { get; set; }
    public string RuntimeMinutes { get; set; }
    public string Genres { get; set; }
  }
}
