using System.Collections.Generic;
/// https://www.imdb.com/interfaces/
namespace Core.Models.Title {
  public class Rating {
    public string TitleId { get; set; }
    public TitleAgg Title { get; set; }
    public float AverageRating { get; set; }
    public int Votes { get; set; }
  }
}
