using Microsoft.AspNetCore.SignalR.Client;
using R_BlazorCommon.Constants;
using R_BlazorFrontEnd.Configurations;

namespace BlazorMenu.Extensions
{
    public static class HubExtensions
    {
        public static HubConnection TryInitialize(this HubConnection hubConnection)
        {
            if (hubConnection == null)
            {
                var baseUrl = R_FrontConfig.R_GetConfigAsString("R_ServiceUrlSection:R_DefaultServiceUrl");
                baseUrl = baseUrl.LastIndexOf("/") < 0 ? baseUrl : baseUrl.Substring(0, baseUrl.LastIndexOf("/"));

                hubConnection = new HubConnectionBuilder()
                                  .WithUrl(new Uri(baseUrl + BlazorMenuConstants.SignalR.HubUrl), options =>
                                  {
                                      options.SkipNegotiation = true;
                                      options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets;
                                  })
                                  .WithAutomaticReconnect()
                                  .Build();
            }

            return hubConnection;
        }
    }
}