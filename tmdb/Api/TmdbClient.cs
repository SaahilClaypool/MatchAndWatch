using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Infrastructure;

namespace Tmdb.Api {
    public class TmdbClient {
        private string ApiKey { get; }
        static readonly HttpClient client = new HttpClient();

        private static string BaseUrl => "https://api.themoviedb.org/3";

        public static TmdbClient Default() {
            return new(Environment.GetEnvironmentVariable("TMDB_API_KEY")!);
        }

        public TmdbClient(string apiKey) {
            ApiKey = apiKey;
        }

        public async Task<T> MakeRequest<T>(string path) {
            var url = ConstructUrl(path);
            try {
                string responseBody = await client.GetStringAsync(url);
                return JsonSerializer.Deserialize<T>(responseBody);
            }
            catch (HttpRequestException e) {
                Console.WriteLine($"Failed to make API request for {path}");
                Console.WriteLine("Message: {0} ", e.Message);
            }
            throw new Exception();
        }

        private string ConstructUrl(string path) => $"{BaseUrl}/{path}?api_key={ApiKey}";
    }
}
