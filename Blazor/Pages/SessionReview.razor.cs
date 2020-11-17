using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using DTO.Session;

using Microsoft.AspNetCore.Components;

namespace Blazor.Pages {
    public partial class SessionReview {
        [Inject] private HttpClient? Http { get; set; }

        [Inject] private NavigationManager? NavigationManager { get; set; }

        [Parameter] public string? SessionId { get; set; }

        FullSessionDTO? Session { get; set; } = null;

        protected override async Task OnInitializedAsync() {
            base.OnInitialized();

            Session = await Http!.GetFromJsonAsync<FullSessionDTO>($"api/Session/{SessionId!}/full");
        }
    }
}
