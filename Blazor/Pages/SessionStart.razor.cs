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

        [Parameter] public string SessionId { get; set; }


        private CreateSessionCommand? Command { get; set; }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            var generesTask = Http!.GetFromJsonAsync<List<string>>("api/Genre");
            if (SessionId is not null) {
                var session = await Http!.GetFromJsonAsync<SessionDTO>($"api/Session/{SessionId}");
                Command = new() {
                    Name = session.Name,
                    Genres = new HashSet<string>(session.Genres)
                };
            }

            // this might be initialized if its from a parameter
            Command ??= new() {
                Name = "",
                Genres = new HashSet<string>()
            };
            Genres = await generesTask;
        }
        enum FormStates {
            GENRES = 0,
            MAX // always last
        }

        private FormStates FormState { get; set; } = FormStates.GENRES;

        private HashSet<string> SelectedGenres { get => (HashSet<string>)Command!.Genres; }
        private IEnumerable<string>? Genres { get; set; }

        private void ToggleSelected(string genre, bool isSelected) {
            if (isSelected) {
                SelectedGenres.Add(genre);
            }
            else {
                SelectedGenres.Remove(genre);
            }
        }

        private async Task<HttpResponseMessage> CreateOrUpdate() {
            if(SessionId is null) {
                return await Http!.PostAsJsonAsync("api/Session", Command);
            } else {
                return await Http!.PutAsJsonAsync($"api/Session/{SessionId}", Command);
            }
        }

        private async Task<bool> Submit() {
            var response = await CreateOrUpdate();
            if (response.IsSuccessStatusCode) {
                var validResponse = await response.Content.ReadFromJsonAsync<CreateSessionResponse>();
                SessionId = validResponse.Id;
                Complete();
                return false;
            }
            // TODO: we should add generic error handling here...
            // raise an exception with an error should display the messages at the top or something
            return false;
        }

        private void Complete() {
            NavigationManager!.NavigateTo($"session/{SessionId!}/show");
        }

        protected string ButtonText {
            get => FormState switch {
                FormStates.GENRES => SessionId is null ? "Submit" : "Update",
                FormStates.MAX => "Start",
                _ => throw new NotImplementedException(),
            };
        }

        private async Task Next() {

            var success = FormState switch {
                FormStates.GENRES => await Submit(),
                _ => throw new NotImplementedException("No such state")
            };


            if (success) {
                FormState += 1;
            }
        }

        private bool ShowStart() => SessionId is not null;
        private void Start() => NavigationManager!.NavigateTo($"/session/{SessionId}/rating/new");
    }
}
