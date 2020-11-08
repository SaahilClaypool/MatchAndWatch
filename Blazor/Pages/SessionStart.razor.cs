using Microsoft.AspNetCore.Components;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;
using System;
using DTO.Session;

namespace Blazor.Pages {
    public partial class SessionStart {
        [Inject] private HttpClient? Http { get; set; }

        [Inject] private NavigationManager? NavigationManager { get; set; }

        private CreateSessionCommand Command { get; set; } = new() {
            Genres = new HashSet<string>(),
            Name = ""
        };

        enum FormStates {
            GENRES = 0,
            MAX // always last
        }

        private FormStates FormState { get; set; } = FormStates.GENRES;

        private HashSet<string> SelectedGenres { get => (HashSet<string>)Command.Genres; }
        private IEnumerable<string>? Genres { get; set; }

        protected override async Task OnInitializedAsync() {
            Genres = await Http!.GetFromJsonAsync<List<string>>("api/Genre");
        }

        private void ToggleSelected(string genre, bool isSelected) {
            if (isSelected) {
                SelectedGenres.Add(genre);
            }
            else {
                SelectedGenres.Remove(genre);
            }
        }

        private async Task<bool> Submit() {
            var response = await Http!.PostAsJsonAsync("api/Session", Command);
            if (response.IsSuccessStatusCode) {
                return true;
            }
            // TODO: we should add generic error handling here...
            // raise an exception with an error should display the messages at the top or something
            return false;
        }

        private bool Complete() {
            NavigationManager!.NavigateTo("session/show");

            // we don't really want to increment after this
            return false;
        }

        protected string ButtonText {
            get => FormState switch {
                FormStates.GENRES => "Submit",
                FormStates.MAX => "Start",
                _ => throw new System.NotImplementedException(),
            };
        }

        private async Task Next() {

            var success = FormState switch {
                FormStates.GENRES => await Submit(),
                FormStates.MAX => Complete(),
                _ => true
            };


            if (success) {
                FormState += 1;
            }
        }
    }
}
