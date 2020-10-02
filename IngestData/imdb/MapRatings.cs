using System.Linq;

using Core.Models;
using Core.Models.Title;

using Shared;

namespace IngestData.imdb {
  class MapRatings : AMapper<RatingsRow, Rating> {
    public override Rating Map(RatingsRow from) {
      Rating rating = new() {
        TitleId = from.Tconst,
        Votes = from.NumVotes,
        AverageRating = from.AverageRating
      };

      return rating;
    }
  }

  public class RatingsRow {
    public string Tconst { get; set; } // title id
    public float AverageRating { get; set; }
    public int NumVotes { get; set; }
  }
}
