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
        private string? PosterPath { get; set; } = null;

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
            PosterPath = MovieInfo?.PosterPartialPath;
            loading = false;
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
            if(loading) {
                return;
            }

            var createCommand = new RatingDTO() { 
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
