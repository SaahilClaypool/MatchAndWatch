using Microsoft.AspNetCore.Components;

using System.Threading.Tasks;

using Blazor.Helpers;

using DTO.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace Blazor.Pages {
    public partial class Login {
        [Inject]
        protected IAuthService AuthServiceInstance { get; set; }
        [Inject]
        protected NavigationManager NavigationManger { get; set; }

        protected LoginDTO LoginModel = new LoginDTO("", "");
        protected bool ShowErrors;
        protected string Error = "";

        protected async Task HandleLogin() {
            ShowErrors = false;

            var result = await this.AuthServiceInstance.Login(LoginModel);

            if (result.Succeeded) {
                this.NavigationManger.NavigateTo("/");
            }
            else {
                Error = "Error";
                ShowErrors = true;
            }
        }

        protected void GoToLogout() {
            NavigationManger.NavigateTo("logout");
        }
    }
}
