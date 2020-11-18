using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using DTO.Rating;

using Extensions;

using Microsoft.AspNetCore.Components;

namespace Blazor.Pages {
    public partial class SessionRating {
        [Inject] private HttpClient? Http { get; set; }
        [Inject] public NavigationManager? NavigationManager { get; set; }

        [Parameter] public string? SessionId { get; set; }
        [Parameter] public string? RatingId { get; set; }

        private bool loading = true;

        public MovieInformationQueryDTO? informationQuery;

        private MovieInformationResponseDTO? MovieInfo { get; set; }
        private List<Task<MovieInformationResponseDTO?>> MovieInfos { get; set; } = new();

        private string MovieTitle => MovieInfo?.MovieTitle ?? "Loading...";
        private string MovieSummary => MovieInfo?.MovieSummary ?? "Loading...";

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            await NextTitle();
        }

        private async Task NextTitle() {
            const int cached = 2;
            if (MovieInfos.Count > 0) {
                MovieInfos.Remove(MovieInfos[0]);
            }
            if (MovieInfos.Count < cached) {
                foreach (var _ in Enumerable.Range(MovieInfos.Count, cached - MovieInfos.Count)) {
                    MovieInfos.Add(FetchTitle());
                }
            }

            var newMovieInfo = await MovieInfos[0];
            if (newMovieInfo is not null && newMovieInfo.MovieId != MovieInfo?.MovieId) {
                MovieInfo = newMovieInfo;
                loading = false;
            }
            else {
                await NextTitle();
            }

        }

        private async Task<MovieInformationResponseDTO?> FetchTitle() {
            informationQuery = new() {
                SessionId = SessionId!
            };
            return await Http!.GetFromJsonAsync<MovieInformationResponseDTO>($"api/Session/{SessionId!}/Rating/");
        }

        protected async Task OnUpvote() {
            await HandleCommand(RatingDTO.Upvote);
        }

        protected async Task OnDownvote() {
            await HandleCommand(RatingDTO.Downvote);
        }

        protected async Task OnPass() {
            await HandleCommand(RatingDTO.Pass);
        }

        private async Task HandleCommand(string type) {
            if (loading) {
                return;
            }

            var createCommand = new RatingDTO() {
                Type = type,
                MovieId = MovieInfo!.MovieId
            };
            loading = true;
            var response = await Http!.PostAsJsonAsync($"api/Session/{SessionId!}/Rating/", createCommand);
            if (response.IsSuccessStatusCode) {
                var responseDTO = await response.Content.ReadFromJsonAsync<CreateRatingResponseDTO>();
                Console.WriteLine($"Response: {responseDTO.ToJson()}");
                await NextTitle();
            }
        }
    }
}
