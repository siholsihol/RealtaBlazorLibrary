using Blazored.LocalStorage;
using BlazorMenu.Constants.Storage;
using Newtonsoft.Json;

namespace BlazorMenu.Services
{
    public class LocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;

        public LocalStorageService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        #region Culture
        public async Task SetCulture(string pcCultureId)
        {
            await _localStorageService.SetItemAsStringAsync(StorageConstants.Culture, pcCultureId);
        }

        public async Task<string> GetCulture()
        {
            return await _localStorageService.GetItemAsStringAsync(StorageConstants.Culture);
        }
        #endregion

        #region CultureInfo
        public async Task SetCultureInfo(Dictionary<string, string> poCultureInfo)
        {
            await _localStorageService.SetItemAsStringAsync(StorageConstants.CultureInfo, JsonConvert.SerializeObject(poCultureInfo));
        }

        public async Task<Dictionary<string, string>> GetCultureInfo()
        {
            var lcCultureInfo = await _localStorageService.GetItemAsStringAsync(StorageConstants.CultureInfo);
            var loCultureInfoResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(lcCultureInfo);

            return loCultureInfoResult;
        }
        #endregion

        public async Task ClearLocalStorage()
        {
            await _localStorageService.RemoveItemsAsync(new string[]
            {
                StorageConstants.AuthToken,
                StorageConstants.Culture,
                StorageConstants.CultureInfo
            });
        }
    }
}
