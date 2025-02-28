using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.Router;
using R_BlazorFrontEnd.Deployment.Interfaces;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTabSetTool
    {
        private readonly NavigationManager _navigationManager;
        //private readonly R_ITenant _tenant;
        private readonly RouteManager _routeManager;
        private readonly IConfiguration _configuration;
        private readonly R_IAssemblyDataProvider _assemblyDataProvider;

        public MenuTabSetTool(
            NavigationManager navigationManager,
            RouteManager routeManager,
            IConfiguration configuration,
            R_IAssemblyDataProvider assemblyDataProvider)
        {
            _navigationManager = navigationManager;
            //_tenant = tenant;
            _routeManager = routeManager;
            _configuration = configuration;
            _assemblyDataProvider = assemblyDataProvider;
        }

        public List<MenuTab> Tabs { get; set; } = new();
        public int ActiveTabIndex = 0;

        public async Task AddTab(string title, string url, string pcAccess = "")
        {
            try
            {
                var lcDeploymentUrl = GetDeploymentServiceUrl();

                if (!string.IsNullOrWhiteSpace(lcDeploymentUrl))
                {
                    var newRelativeUri = $"{url}Front";

                    await _assemblyDataProvider.CheckAssemblyAsync(newRelativeUri);
                }
                else
                {
                    var llExistRoute = _routeManager.Routes.Any(x => x.UriSegments.Any(y => string.Equals(y, url, StringComparison.InvariantCultureIgnoreCase)));
                    if (!llExistRoute)
                        throw new Exception($"{url} not found.");
                }

                //var lcUrlTenant = _tenant.Identifier + "/" + url;

                Tabs.ForEach(x =>
                {
                    x.IsActive = false;
                });

                MenuTab loNewTab = null;
                var selTab = Tabs.FirstOrDefault(m => m.Url == url && (m.Title == title || string.IsNullOrEmpty(m.Title)));
                if (selTab == null)
                {
                    var lcAccess = string.IsNullOrWhiteSpace(pcAccess) ? "V" : pcAccess;

                    loNewTab = new MenuTab
                    {
                        Url = url,
                        Title = title,
                        IsActive = true,
                        Access = lcAccess
                    };

                    Tabs.Add(loNewTab);
                }
                else
                {
                    if (string.IsNullOrEmpty(selTab.Title))
                        selTab.Title = title;

                    selTab.IsActive = true;
                }

                _navigationManager.NavigateTo(url);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetDeploymentServiceUrl()
        {
            var lcUrl = string.Empty;

            try
            {
                var loUrls = _configuration.GetSection("R_ServiceUrlSection").Get<Dictionary<string, string>>();

                lcUrl = loUrls.FirstOrDefault(x => x.Key == "R_DeploymentServiceUrl").Value;
            }
            catch (Exception)
            {
            }

            return lcUrl;
        }
    }
}
