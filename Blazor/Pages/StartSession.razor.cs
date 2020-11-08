using Microsoft.AspNetCore.Components;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blazor.Pages {
    public partial class StartSession {
        [Inject]
        protected HttpClient Http { get; set; }

        enum FormStates {
            GENRES
        }

        private FormStates FormState { get; set; } = FormStates.GENRES;

        private IEnumerable<string> SelectedGenres { get; set; } = new List<string>();
        private IEnumerable<string>? Genres { get; set; }
    }
}
