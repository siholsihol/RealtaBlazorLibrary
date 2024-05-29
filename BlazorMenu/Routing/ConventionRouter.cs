using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using R_CommonFrontBackAPI;
using System.Runtime.Loader;

namespace BlazorMenu.Routing
{
    public class ConventionRouter : IComponent, IHandleAfterRender, IDisposable
    {
        RenderHandle _renderHandle;
        bool _navigationInterceptionEnabled;
        string _location;

        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private INavigationInterception NavigationInterception { get; set; }
        [Inject] RouteManager RouteManager { get; set; }
        //[Inject] private HttpClient HttpClient { get; set; }
        [Inject] private Interop DOMinterop { get; set; }
        [Inject] private IHttpClientFactory HttpClientFactory { get; set; }

        [Parameter] public RenderFragment NotFound { get; set; }
        [Parameter] public RenderFragment<RouteData> Found { get; set; }

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
            _location = NavigationManager.Uri;
            NavigationManager.LocationChanged += HandleLocationChanged;
        }

        public async Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (Found == null)
            {
                throw new InvalidOperationException($"The {nameof(ConventionRouter)} component requires a value for the parameter {nameof(Found)}.");
            }

            if (NotFound == null)
            {
                throw new InvalidOperationException($"The {nameof(ConventionRouter)} component requires a value for the parameter {nameof(NotFound)}.");
            }

            RouteManager.Initialize();
            await Refresh();
        }

        public Task OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= HandleLocationChanged;
        }

        private void HandleLocationChanged(object sender, LocationChangedEventArgs args)
        {
            _location = args.Location;

            var task = Refresh();
            Task.Run(() => task);
        }

        private async Task Refresh()
        {
            var relativeUri = NavigationManager.ToBaseRelativePath(_location).Replace("#", "");
            var parameters = ParseQueryString(relativeUri) ?? new Dictionary<string, object>();

            if (relativeUri.IndexOf('?') > -1)
            {
                relativeUri = relativeUri.Substring(0, relativeUri.IndexOf('?'));
            }

            var segments = relativeUri.Trim().Split('/', StringSplitOptions.RemoveEmptyEntries);
            var matchResult = RouteManager.Match(segments);

            if (matchResult.IsMatch)
            {
                var routeData = new RouteData(
                    matchResult.MatchedRoute.Handler,
                    parameters);

                _renderHandle.Render(Found(routeData));
            }
            else
            {
                try
                {
                    var httpClient = HttpClientFactory.CreateClient("R_DeploymentServiceUrl");
                    var url = $"api/File/DownloadFile?pcFileName={relativeUri}Front.dll";
                    var bytePackage = await httpClient.GetByteArrayAsync(url);
                    var packages = R_NetCoreUtility.R_DeserializeObjectFromByte<List<Package>>(bytePackage);

                    //add to context all referenced assembly and create css reference
                    var referencePackages = packages.Where(x => x.IsReference).ToList();

                    foreach (var item in referencePackages)
                    {
                        AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(item.Content));

                        var fileNameWithoutExt = item.FileName.Substring(0, item.FileName.LastIndexOf("."));
                        var referenceCssUrl = httpClient.BaseAddress.ToString() + $"files/{fileNameWithoutExt}.styles.css";
                        await DOMinterop.IncludeLink(fileNameWithoutExt, referenceCssUrl);
                    }

                    //add to context main assembly
                    var mainPackage = packages.Where(x => !x.IsReference).FirstOrDefault();
                    var assembly = AssemblyLoadContext.Default.LoadFromStream(new MemoryStream(mainPackage.Content));
                    var componentType = assembly.GetType(relativeUri + "Front." + relativeUri);

                    //add css main assembly
                    var cssUrl = httpClient.BaseAddress.ToString() + $"files/{relativeUri}.styles.css";
                    await DOMinterop.IncludeLink(relativeUri, cssUrl);

                    if (componentType != null)
                    {
                        var routeData = new RouteData(
                        componentType,
                        parameters);

                        _renderHandle.Render(Found(routeData));
                    }
                    else
                        _renderHandle.Render(NotFound);
                }
                catch (Exception ex)
                {
                    _renderHandle.Render(NotFound);
                    Console.WriteLine(ex.ToString());
                    throw ex;
                }
            }
        }

        private Dictionary<string, object> ParseQueryString(string uri)
        {
            var querystring = new Dictionary<string, object>();

            foreach (var kvp in uri.Substring(uri.IndexOf("?") + 1).Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (kvp != "" && kvp.Contains("="))
                {
                    var pair = kvp.Split('=');
                    querystring.Add(pair[0], pair[1]);
                }
            }

            return querystring;
        }
    }
}
