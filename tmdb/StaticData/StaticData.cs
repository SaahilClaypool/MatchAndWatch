using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Tmdb.StaticData {
    public class StaticData {
        DateTime DateTime { get; set; }
        public string DateString => string.Format("{0:MM_dd_yyyy}", DateTime);

        public static string DataDir => $"{Environment.CurrentDirectory}/data";
        public static string[] Files => new string[] {
      "movie_ids",
      "tv_series_ids",
      "person_ids",
      "collection_ids",
      "tv_network_ids",
      "keyword_ids",
      "production_company_ids"
     };

        public StaticData(DateTime dateTime) {
            DateTime = dateTime;
        }

        public async Task DownloadAll() {
            var downloads = Files.Select(file => DownloadFile(file));
            await Task.WhenAll(downloads);
        }

        /// <returns>string filename of where the file is downloaded to</returns>
        public async Task<string> DownloadFile(string filePrefix) {
            var fileName = $"{filePrefix}_{DateString}.json";
            System.Console.WriteLine($"Downloading file {fileName}");
            var url = $"http://files.tmdb.org/p/exports/{fileName}.gz";
            var outputFile = $"{DataDir}/{fileName}";

            var getInfo = new ProcessStartInfo("wget") {
                Arguments = $"-O {outputFile}.gz {url}"
            };
            var wget = Process.Start(getInfo);
            await wget.WaitForExitAsync();

            var gunzipInfo = new ProcessStartInfo("gunzip") {
                Arguments = $"{outputFile}.gz"
            };
            var gunzip = Process.Start(gunzipInfo);
            await gunzip.WaitForExitAsync();

            return outputFile;
        }
    }
}
