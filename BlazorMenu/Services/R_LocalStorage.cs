using Blazored.LocalStorage;
using R_BlazorFrontEnd.Interfaces;

namespace BlazorMenu.Services
{
    public class R_LocalStorage : R_ILocalStorage
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;

        public R_LocalStorage(ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService)
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
        }

        #region SetItem
        public ValueTask SetItemAsync<TValue>(string pcKey, TValue poValue)
        {
            return _localStorageService.SetItemAsync(pcKey, poValue);
        }

        public void SetItem<TValue>(string pcKey, TValue poValue)
        {
            _syncLocalStorageService.SetItem(pcKey, poValue);
        }
        #endregion

        #region GetItem
        public ValueTask<TValue> GetItemAsync<TValue>(string pcKey)
        {
            return _localStorageService.GetItemAsync<TValue>(pcKey);
        }

        public TValue GetItem<TValue>(string pcKey)
        {
            return _syncLocalStorageService.GetItem<TValue>(pcKey);
        }
        #endregion

        #region RemoveItem
        public ValueTask RemoveItemAsync(string pcKey)
        {
            return _localStorageService.RemoveItemAsync(pcKey);
        }

        public void RemoveItem(string pcKey)
        {
            _syncLocalStorageService.RemoveItem(pcKey);
        }
        #endregion

        #region RemoveItems
        public ValueTask RemoveItemsAsync(string[] pcKey)
        {
            return _localStorageService.RemoveItemsAsync(pcKey);
        }

        public void RemoveItems(string[] pcKey)
        {
            _syncLocalStorageService.RemoveItems(pcKey);
        }
        #endregion
    }
}
