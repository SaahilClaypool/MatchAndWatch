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
        private string PosterPath { get; set; } = "https://image.tmdb.org/t/p/w500/wwemzKWzjKYJFfCeiB57q3r4Bcm.png";

        private bool loading = true; 

        public MovieInformationQueryDTO? informationQuery;

        private MovieInformationResponseDTO? MovieInfo { get; set; }

        private string MovieTitle => MovieInfo?.MovieTitle ?? "Loading...";
        private string MovieSummary => MovieInfo?.MovieSummary ?? "Loading...";

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            await FetchTitle();
        }

        private async Task FetchTitle() {
            informationQuery = new() {
                SessionId = SessionId
            };
            MovieInfo = await Http!.GetFromJsonAsync<MovieInformationResponseDTO>($"api/Session/{SessionId!}/Rating/");
            if (MovieInfo?.PosterPartialPath is string poster) {
                PosterPath = poster;
            }
            loading = false;
        }

        protected async Task OnUpvote() {
            await HandleCommand(CreateRatingDTO.Upvote);
        }

        protected async Task OnDownvote() {
            await HandleCommand(CreateRatingDTO.Downvote);
        }

        protected async Task OnPass() {
            await HandleCommand(CreateRatingDTO.Pass);
        }

        private async Task HandleCommand(string type) {
            if(loading) {
                return;
            }

            var createCommand = new CreateRatingDTO() { 
                Type = type,
                MovieId = MovieInfo!.MovieId
            } ;
            loading = true;
            var response = await Http!.PostAsJsonAsync($"api/Session/{SessionId!}/Rating/", createCommand);
            if (response.IsSuccessStatusCode) {
                var responseDTO = await response.Content.ReadFromJsonAsync<CreateRatingResponseDTO>();
                Console.WriteLine($"Response: {responseDTO.ToJson()}");
                await FetchTitle();
            }
        }
    }
}
