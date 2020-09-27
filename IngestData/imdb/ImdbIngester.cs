using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Core.Models.Title;

using CsvHelper;

using Infrastructure;
using Infrastructure.Data;

using Shared;

namespace IngestData.imdb {
    public class ImdbIngester : AIngester {
        readonly ImdbTsvPaths Paths;
        public ImdbIngester(ImdbTsvPaths paths) {
            Paths = paths;
        }

        public override async Task Ingest() {
            await IngestBasics();
        }

        public static CsvReader Reader(string path) {
            var reader = new StreamReader(path);
            ;
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Configuration.Delimiter = "\t";
            csv.Configuration.PrepareHeaderForMatch = (header, _) => header.ToLower();
            csv.Configuration.BadDataFound = record => record.RawRecord.Dbg();
            csv.Configuration.MissingFieldFound = (fields, index, _context) => $"{index} : {fields.ToJson()}".Dbg();
            return csv;
        }

        private async Task IngestBasics() {
            var csv = Reader(Paths.TitleBasics);
            var mapper = new MapBasics();
            var dbRecords = csv.GetRecords<BasicsRow>()
            .Select(
                record => mapper.Map(record)
            );

            await IngestRecords(dbRecords, Context.TitleAggs);
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
