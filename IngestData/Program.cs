using tmdb.StaticData;
using System;
using System.Threading.Tasks;

namespace IngestData {
  class Program {
    static async Task Main(string[] _args) {
      var tmdbData = new StaticData(DateTime.Now.AddDays(-1));
      await tmdbData.DownloadAll();
    }
  }
}
