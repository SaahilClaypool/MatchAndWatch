using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Extensions;

using Microsoft.Extensions.Caching.Memory;

namespace Tmdb.Api {
    public class TmdbClient {
        private string ApiKey { get; }
        private IMemoryCache? Cache { get; set; }
        static readonly HttpClient client = new HttpClient();

        private static string BaseUrl => "https://api.themoviedb.org/3";

        public static TmdbClient Default() {
            return new(Environment.GetEnvironmentVariable("TMDB_API_KEY")!);
        }

        public static TmdbClient Default(IMemoryCache cache) {
            return new(Environment.GetEnvironmentVariable("TMDB_API_KEY")!) {
                Cache = cache
            };
        }

        public TmdbClient(string apiKey) {
            ApiKey = apiKey;
        }
        
        public async Task<T> MakeRequest<T>(string path) {
            if(Cache?.Get(path) is T result) {
                return result;
            }

            var url = ConstructUrl(path);
            try {
                string responseBody = await client.GetStringAsync(url);
                var response = JsonSerializer.Deserialize<T>(responseBody);
                Cache?.Set(path, response);
                return response;
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
