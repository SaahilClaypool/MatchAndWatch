using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Blazor.Helpers;

using DTO.Login;
using DTO.Session;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Components {
    public partial class UserIndex {
        [Inject] protected IAuthService AuthServiceInstance { get; set; }
        [Inject] protected NavigationManager NavigationManger { get; set; }
        [Inject] protected HttpClient? Http { get; set; }
        IEnumerable<SessionDTO>? _sessions = null;

        protected override async Task OnInitializedAsync() {
            base.OnInitialized();

            // _sessions = await Http!.GetFromJsonAsync<IEnumerable<SessionDTO>>("api/session/");
            _sessions = await Http!.GetFromJsonAsync<IEnumerable<SessionDTO>>("api/session/participating");
        }

        public static string GenresString(SessionDTO session) => string.Join(", ", session.Genres);
    }
}
