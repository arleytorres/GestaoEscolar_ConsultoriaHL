using BlazorApp1.Services;
using Microsoft.JSInterop;

namespace BlazorApp1.Components.Layout
{
    public partial class MainLayout
    {

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var localService = new LocalStorageService(JSRuntime);
                await localService.Load();
                StateHasChanged();
            }
        }
    }
}
