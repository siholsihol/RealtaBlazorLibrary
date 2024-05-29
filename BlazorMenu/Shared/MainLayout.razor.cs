using BlazorMenu.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorMenu.Shared
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] private HttpInterceptorService _httpInterceptorService { get; set; }

        protected override void OnInitialized()
        {
            _httpInterceptorService.RegisterEvent();
        }
    }
}
