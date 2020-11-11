using Microsoft.AspNetCore.Components;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using DTO.Rating;

namespace Blazor.Pages {
    public partial class SessionRating {
        [Inject] private HttpClient? Http { get; set; }
        [Inject] private NavigationManager? NavigationManager { get; set; }

        [Parameter] public string? SessionId { get; set; }
        [Parameter] public string? RatingId { get; set; }
        private string PosterPath { get; set; } = "https://image.tmdb.org/t/p/w500/wwemzKWzjKYJFfCeiB57q3r4Bcm.png";

        private MovieInformationQueryDTO? InformationQuery { get; set; }
        private MovieInformationResponseDTO? MovieInfo { get; set; }


        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            InformationQuery = new() {
                SessionId = SessionId
            };
            MovieInfo = await Http!.GetFromJsonAsync<MovieInformationResponseDTO>($"api/Session/{SessionId!}/Rating/");
        }
    }
}
