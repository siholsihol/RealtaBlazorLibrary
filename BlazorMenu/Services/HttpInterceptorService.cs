using BlazorMenu.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Controls.Notification;
using Toolbelt.Blazor;

namespace BlazorMenu.Services
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _httpClientInterceptor;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _stateProvider;
        private readonly R_NotificationService _notificationService;
        private readonly IWebAssemblyHostEnvironment _webAssemblyHostEnvironment;

        public HttpInterceptorService(
            HttpClientInterceptor httpClientInterceptor,
            NavigationManager navigationManager,
            AuthenticationStateProvider stateProvider,
            R_NotificationService notificationService,
            IWebAssemblyHostEnvironment webAssemblyHostEnvironment)
        {
            _httpClientInterceptor = httpClientInterceptor;
            _navigationManager = navigationManager;
            _stateProvider = stateProvider;
            _notificationService = notificationService;
            _webAssemblyHostEnvironment = webAssemblyHostEnvironment;
        }

        public void RegisterEvent()
        {
            _httpClientInterceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
            _httpClientInterceptor.AfterSendAsync += InterceptAfterHttpAsync;
        }

        public Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (!absPath.Contains("token") && !absPath.Contains("accounts"))
            {
                try
                {
                    //var token = await _authenticationManager.TryRefreshToken();
                    //if (!string.IsNullOrEmpty(token))
                    //{
                    //    e.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    //}
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    //await _authenticationManager.Logout();
                    _navigationManager.NavigateTo("/");
                }
            }

            return Task.CompletedTask;
        }

        public async Task InterceptAfterHttpAsync(object sender, HttpClientInterceptorEventArgs e)
        {
            var absPath = e.Request.RequestUri.AbsolutePath;
            if (absPath.Contains("R_RefreshToken") && !e.Response.IsSuccessStatusCode)
            {
                await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsLoggedOut();
                _navigationManager.NavigateTo("/");

                _notificationService.Error("Invalid refresh token");
            }
        }

        public void DisposeEvent() => _httpClientInterceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}
