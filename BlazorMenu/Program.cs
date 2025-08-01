using BlazorMenu.Components.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls.Extensions;
using R_BlazorFrontEnd.FileConverter;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using R_BlazorStartup;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<BlazorMenu.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.R_AddBlazorFrontEndControls();

builder.R_RegisterBlazorServices(option =>
{
    if (!builder.HostEnvironment.IsDevelopment())
        option.R_WithMultiTenant();
});

builder.Services.R_AddBlazorMenuServices();

builder.Services.AddTransient<R_IFileConverter, R_FileConverter>();
builder.Services.AddSingleton<R_IFileDownloader, R_FileDownloader>();

Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", builder.HostEnvironment.Environment);

var host = builder.Build();

host.R_SetupBlazorService();

await host.R_UseBlazorMenuServices();

await host.RunAsync();
