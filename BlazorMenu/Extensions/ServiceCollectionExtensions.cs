﻿using System.Globalization;
using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Services;
using BlazorMenu.Shared;
using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Controls.Menu;
using R_BlazorFrontEnd.Interfaces;

namespace BlazorMenu.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection R_AddBlazorMenuServices(this IServiceCollection services)
        {
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, BlazorMenuAuthenticationStateProvider>();

            services.AddSingleton(typeof(R_ILocalizer<>), typeof(R_Localizer<>));
            services.AddSingleton<R_ILocalizer, R_Localizer>();
            services.AddSingleton<R_IMainBody, MainBody>();

            services.AddScoped<MenuTabSetTool>();

            services.AddTransient<R_IMenuService, R_MenuService>();

            services.AddSingleton<IClientHelper, U_GlobalVar>();

            services.AddScoped<R_ILocalStorage, R_LocalStorage>();
            services.AddScoped<BlazorMenuLocalStorageService>();

            services.AddTransient<HttpInterceptorService>();
            services.AddSingleton<R_IEnvironment, BlazorMenuEnvironmentService>();

            return services;
        }

        internal static async Task R_UseBlazorMenuServices(this WebAssemblyHost host)
        {
            var loLocalStorage = host.Services.GetRequiredService<BlazorMenuLocalStorageService>();
            var lcCulture = await loLocalStorage.GetCultureAsync();

            CultureInfo loCulture = new CultureInfo("en");
            if (!string.IsNullOrWhiteSpace(lcCulture))
                loCulture = new CultureInfo(lcCulture);

            CultureInfo.DefaultThreadCurrentCulture = loCulture;
            CultureInfo.DefaultThreadCurrentUICulture = loCulture;
        }
    }
}
