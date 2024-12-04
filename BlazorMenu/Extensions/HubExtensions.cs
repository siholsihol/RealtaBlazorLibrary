using Microsoft.AspNetCore.SignalR.Client;
using R_BlazorCommon.Constants;
using R_BlazorFrontEnd.Configurations;

namespace BlazorMenu.Extensions
{
    public static class HubExtensions
    {
        //public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager, ILocalStorageService localStorage)
        //{
        //    if (hubConnection == null)
        //    {
        //        hubConnection = new HubConnectionBuilder()
        //                          .WithUrl(new Uri("http://localhost:5072" + ApplicationConstants.SignalR.HubUrl), options =>
        //                          {
        //                              options.AccessTokenProvider = async () => await localStorage.GetItemAsync<string>("authToken");
        //                          })
        //                          .WithAutomaticReconnect()
        //                          .Build();
        //    }

        //    return hubConnection;
        //}

        public static HubConnection TryInitialize(this HubConnection hubConnection)
        {
            if (hubConnection == null)
            {
                var baseUrl = R_FrontConfig.R_GetConfigAsString("R_ServiceUrlSection:R_DefaultServiceUrl");
                baseUrl = baseUrl.LastIndexOf("/") < 0 ? baseUrl : baseUrl.Substring(0, baseUrl.LastIndexOf("/"));

                hubConnection = new HubConnectionBuilder()
                                  .WithUrl(new Uri(baseUrl + BlazorMenuConstants.SignalR.HubUrl))
                                  .WithAutomaticReconnect()
                                  .Build();
            }

            return hubConnection;
        }

        //public static HubConnection TryInitialize(this HubConnection hubConnection)
        //{
        //    if (hubConnection == null)
        //    {
        //        hubConnection = new HubConnectionBuilder()
        //                          .WithUrl(
        //                            new Uri("http://localhost:5127" + "/blazormenuhub"),
        //                            option => option.AccessTokenProvider = () => Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRmYzEyODFiLTZhZDQtNDUxZS04YzRjLTU2OGY5NDk5NTQ0YiIsInN1YiI6ImRmYzEyODFiLTZhZDQtNDUxZS04YzRjLTU2OGY5NDk5NTQ0YiIsImp0aSI6IjVkMjZlYmMiLCJhdWQiOlsiaHR0cDovL2xvY2FsaG9zdDoyMjE4OSIsImh0dHBzOi8vbG9jYWxob3N0OjQ0MzUxIiwiaHR0cDovL2xvY2FsaG9zdDo1MTI3Il0sIm5iZiI6MTczMTI4ODYxNSwiZXhwIjoxNzM5MjM3NDE1LCJpYXQiOjE3MzEyODg2MTYsImlzcyI6ImRvdG5ldC11c2VyLWp3dHMifQ.luewkblG_iNjFJKe5AkDWIvxesDUrScoP-nU5f99Fv0"))
        //                          .WithAutomaticReconnect()
        //                          .Build();
        //    }

        //    return hubConnection;
        //}
    }
}