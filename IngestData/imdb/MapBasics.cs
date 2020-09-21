using Core.Models.Title;

using System.Linq;
using Shared;

namespace IngestData.imdb {
    class MapBasics : AMapper<BasicsCSV, TitleAgg> {
        public override TitleAgg Map(BasicsCSV from) {
            var result = from.EndYear == "\\N";
            TitleAgg title = new() {
                Id = from.Tconst,
                Type = from.TitleType,
                Name = from.PrimaryTitle,
                ReleaseYear = ParseYear(from.StartYear),
                EndYear = ParseYear(from.EndYear),
                RunTime = ParseYear(from.RuntimeMinutes) ?? 0,
            };

            title.Genres = from.Genres.Split(";").Select(genre => new Genre() {
                GenreName = genre,
                TitleAgg = title
            }).ToList();

            return title;
        }

        static int? ParseYear(string year) => (year.Equals("\\N") ? null : int.Parse(year));
    }

    public class BasicsCSV {
        public string Tconst { get; set; } // id
        public string TitleType { get; set; }
        public string PrimaryTitle { get; set; }
        public string OriginalTitle { get; set; }
        public bool IsAdult { get; set; }
        public string StartYear { get; set; }
        public string EndYear { get; set; }
        public string RuntimeMinutes { get; set; }
        public string Genres { get; set; }
    }
}
