using BlazorClientHelper;
using Blazored.LocalStorage;
using BlazorMenu.Authentication;
using BlazorMenu.Constants.Storage;
using BlazorMenu.Services;
using BlazorMenu.Shared;
using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Controls.Menu;
using R_BlazorFrontEnd.Interfaces;
using System.Globalization;

namespace BlazorMenu.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection R_AddBlazorFrontEnd(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, BlazorMenuAuthenticationStateProvider>();

            services.AddSingleton(typeof(R_ILocalizer<>), typeof(R_Localizer<>));
            services.AddSingleton<R_ILocalizer, R_Localizer>();
            services.AddSingleton<R_IMainBody, MainBody>();

            services.AddSingleton<MenuTabSetTool>();

            services.AddSingleton<R_IMenuService, R_MenuService>();

            services.AddSingleton<IClientHelper, U_GlobalVar>();

            return services;
        }

        internal static async Task R_UseBlazorFrontEnd(this WebAssemblyHost host)
        {
            var loLocalStorage = host.Services.GetRequiredService<ILocalStorageService>();
            var lcCulture = await loLocalStorage.GetItemAsStringAsync(StorageConstants.Culture);

            CultureInfo loCulture = new CultureInfo("en");
            if (!string.IsNullOrWhiteSpace(lcCulture))
                loCulture = new CultureInfo(lcCulture);

            CultureInfo.DefaultThreadCurrentCulture = loCulture;
            CultureInfo.DefaultThreadCurrentUICulture = loCulture;
        }
    }
}
