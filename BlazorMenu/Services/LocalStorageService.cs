using Blazored.LocalStorage;
using BlazorMenu.Constants.Storage;
using Newtonsoft.Json;

namespace BlazorMenu.Services
{
    public class LocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly ISyncLocalStorageService _syncLocalStorageService;

        public LocalStorageService(ILocalStorageService localStorageService,
            ISyncLocalStorageService syncLocalStorageService)
        {
            _localStorageService = localStorageService;
            _syncLocalStorageService = syncLocalStorageService;
        }

        #region Culture
        public async Task SetCultureAsync(string pcCultureId)
        {
            await _localStorageService.SetItemAsStringAsync(StorageConstants.Culture, pcCultureId);
        }

        public async Task<string> GetCultureAsync()
        {
            return await _localStorageService.GetItemAsStringAsync(StorageConstants.Culture);
        }

        public void SetCulture(string pcCultureId)
        {
            _syncLocalStorageService.SetItemAsString(StorageConstants.Culture, pcCultureId);
        }

        public string GetCulture()
        {
            return _syncLocalStorageService.GetItemAsString(StorageConstants.Culture);
        }
        #endregion

        #region CultureInfo
        public async Task SetCultureInfoAsync(Dictionary<string, string> poCultureInfo)
        {
            await _localStorageService.SetItemAsStringAsync(StorageConstants.CultureInfo, JsonConvert.SerializeObject(poCultureInfo));
        }

        public async Task<Dictionary<string, string>> GetCultureInfoAsync()
        {
            var lcCultureInfo = await _localStorageService.GetItemAsStringAsync(StorageConstants.CultureInfo);
            var loCultureInfoResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(lcCultureInfo);

            return loCultureInfoResult;
        }

        public void SetCultureInfo(Dictionary<string, string> poCultureInfo)
        {
            _syncLocalStorageService.SetItemAsString(StorageConstants.CultureInfo, JsonConvert.SerializeObject(poCultureInfo));
        }

        public Dictionary<string, string> GetCultureInfo()
        {
            var lcCultureInfo = _syncLocalStorageService.GetItemAsString(StorageConstants.CultureInfo);
            var loCultureInfoResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(lcCultureInfo);

            return loCultureInfoResult;
        }
        #endregion

        public async Task ClearLocalStorageAsync()
        {
            await _localStorageService.RemoveItemsAsync(new string[]
            {
                StorageConstants.AuthToken,
                StorageConstants.Culture,
                StorageConstants.CultureInfo
            });
        }

        public void ClearLocalStorage()
        {
            _syncLocalStorageService.RemoveItems(new string[]
            {
                StorageConstants.AuthToken,
                StorageConstants.Culture,
                StorageConstants.CultureInfo
            });
        }
    }
}
