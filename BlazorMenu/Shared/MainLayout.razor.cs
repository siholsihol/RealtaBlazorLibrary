using BlazorMenu.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorMenu.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }

        private async Task Logout()
        {
            await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsLoggedOut();

            _navigationManager.NavigateTo("/");
        }

        private bool _iconMenuActive { get; set; }
        private string IconMenuCssClass => _iconMenuActive ? "width: 80px;" : null;

        protected void ToggleIconMenu(bool iconMenuActive)
        {
            _iconMenuActive = iconMenuActive;
        }
    }
}
