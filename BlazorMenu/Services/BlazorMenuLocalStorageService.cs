using BlazorMenu.Constants.Storage;
using Newtonsoft.Json;
using R_BlazorFrontEnd.Interfaces;

namespace BlazorMenu.Services
{
    public class BlazorMenuLocalStorageService
    {
        private readonly R_ILocalStorage _localStorage;

        public BlazorMenuLocalStorageService(R_ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        #region Culture
        public ValueTask SetCultureAsync(string pcCultureId)
        {
            return _localStorage.SetItemAsync<string>(StorageConstants.Culture, pcCultureId);
        }

        public ValueTask<string> GetCultureAsync()
        {
            return _localStorage.GetItemAsync<string>(StorageConstants.Culture);
        }

        public string GetCulture()
        {
            return _localStorage.GetItem<string>(StorageConstants.Culture);
        }
        #endregion

        #region CultureInfo
        public ValueTask SetCultureInfoAsync(Dictionary<string, string> poCultureInfo)
        {
            return _localStorage.SetItemAsync<string>(StorageConstants.CultureInfo, JsonConvert.SerializeObject(poCultureInfo));
        }

        public async ValueTask<Dictionary<string, string>> GetCultureInfoAsync()
        {
            var lcCultureInfo = await _localStorage.GetItemAsync<string>(StorageConstants.CultureInfo);
            var loCultureInfoResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(lcCultureInfo);

            return loCultureInfoResult;
        }

        public Dictionary<string, string> GetCultureInfo()
        {
            var lcCultureInfo = _localStorage.GetItem<string>(StorageConstants.CultureInfo);
            var loCultureInfoResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(lcCultureInfo);

            return loCultureInfoResult;
        }
        #endregion

        public ValueTask ClearLocalStorageAsync()
        {
            return _localStorage.RemoveItemsAsync(new string[]
            {
                StorageConstants.AuthToken,
                StorageConstants.Culture,
                StorageConstants.CultureInfo,
                StorageConstants.RefreshToken
            });
        }
    }
}
