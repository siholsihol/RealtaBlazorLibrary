using Microsoft.JSInterop;

namespace BlazorMenu.Routing
{
    public class Interop
    {
        private const string JS_PATH = "./script.js";
        private readonly IJSRuntime _jsRuntime;

        public Interop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        //public Task IncludeLink(string id, string href)
        //{
        //    try
        //    {
        //        _jsRuntime.InvokeVoidAsync("NewLazyLoad.Interop.includeLink", id, href);
        //        return Task.CompletedTask;
        //    }
        //    catch
        //    {
        //        return Task.CompletedTask;
        //    }
        //}

        public async Task IncludeLink(string id, string href)
        {
            IJSObjectReference _jsModule = null;

            try
            {
                _jsModule = await GetJsModuleAsync(_jsRuntime);

                await _jsModule.InvokeVoidAsync("includeLink", id, href);
            }
            catch (Exception ex)
            {
                Console.WriteLine("clickComponent " + ex.Message);
            }
            finally
            {
                if (_jsModule is not null)
                    await _jsModule.DisposeAsync();
            }
        }

        public Task AddLink(string id, string style, string place = "head")
        {
            try
            {
                _jsRuntime.InvokeVoidAsync("NewLazyLoad.Interop.addLink", id, style, place);
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        public Task IncludeScript(string id, string src)
        {
            try
            {
                _jsRuntime.InvokeVoidAsync("NewLazyLoad.Interop.includeScript", id, src);
                return Task.CompletedTask;
            }
            catch
            {
                return Task.CompletedTask;
            }
        }

        private static ValueTask<IJSObjectReference> GetJsModuleAsync(IJSRuntime jSRuntime)
        {
            return jSRuntime.InvokeAsync<IJSObjectReference>("import", JS_PATH);
        }
    }
}
