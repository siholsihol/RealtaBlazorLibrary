using BlazorMenu.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls.Extensions;
using R_BlazorFrontEnd.FileConverter;
using R_BlazorStartup;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<BlazorMenu.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.R_AddBlazorFrontEndControls();

builder.R_RegisterBlazorServices();

builder.Services.R_AddBlazorFrontEnd();

builder.Services.AddTransient<R_IFileConverter, R_FileConverter>();

var host = builder.Build();

host.R_SetupBlazorService();

await host.R_UseBlazorFrontEnd();

await host.RunAsync();
