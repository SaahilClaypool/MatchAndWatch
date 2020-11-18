using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

// Based on https://github.com/dotnet/aspnetcore/blob/master/src/Components/Authorization/src/AuthorizeViewCore.cs
namespace Blazor.Components.Spinner {

    /// <summary>
    /// Use by wrapping code in <Spinner IsLoaded=@variable></Spinner>
    /// </summary>
    public class Spinner : ComponentBase {
        [Parameter] public RenderFragment? ChildContent { get; set; } = null;
        [Parameter] public RenderFragment? Loading { get; set; } = null;
        [Parameter] public RenderFragment? Loaded { get; set; } = null;
        [Parameter] public bool IsLoaded { get; set; }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            // We're using the same sequence number for each of the content items here
            // so that we can update existing instances if they are the same shape

            if (IsLoaded) {
                builder.AddContent(0, Loaded ?? ChildContent);
            }
            else {
                var loading = Loading ?? DefaultLoading();
                builder.AddContent(0, loading);
            }
        }

        private static RenderFragment DefaultLoading() {
            static void fragment(RenderTreeBuilder builder) {
                builder.OpenElement(1, "div");
                builder.AddContent(2, "Loading...");
                builder.CloseElement();
            }
            return fragment;
        }
    }
}
