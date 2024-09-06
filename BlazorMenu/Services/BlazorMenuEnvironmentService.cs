using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using R_BlazorFrontEnd.Interfaces;

namespace BlazorMenu.Services
{
    public class BlazorMenuEnvironmentService : R_IEnvironment
    {
        private readonly IWebAssemblyHostEnvironment _environment;

        public string EnvironmentName => _environment.Environment;

        public bool IsDevelopment => _environment.IsDevelopment();

        public bool IsStaging => _environment.IsStaging();

        public bool IsProduction => _environment.IsProduction();

        public BlazorMenuEnvironmentService(IWebAssemblyHostEnvironment environment)
        {
            _environment = environment;
        }
    }
}
