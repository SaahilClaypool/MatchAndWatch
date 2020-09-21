using System.Globalization;
using System.IO;

using Shared;
using CsvHelper;

namespace IngestData.imdb {
    public class ImdbIngester {
        readonly ImdbTsvPaths Paths;
        public ImdbIngester(ImdbTsvPaths paths) {
            Paths = paths;
        }

        public void Ingest() {
            IngestBasics();
        }

        public static CsvReader Reader(string path) {
            var reader = new StreamReader(path);
            ;
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Configuration.Delimiter = "\t";
            csv.Configuration.PrepareHeaderForMatch = (header, _) => header.ToLower();
            return csv;
        }

        private void IngestBasics() {
            var csv = Reader(Paths.TitleBasics);
            foreach (var record in csv.GetRecords<BasicsCSV>()) {
                var dbItem = new MapBasics().Map(record);
                dbItem.Dbg();
            }
        }
    }

    public class ImdbTsvPaths {
        public string NameBasics { get; set; }
        public string TitleBasics { get; set; }
        public string TitleCrew { get; set; }
        public string TitleEpisode { get; set; }
        public string TitlePrincipals { get; set; }
        public string TitleRatings { get; set; }
    }
}
