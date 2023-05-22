﻿using BlazorClientHelper;
using Blazored.LocalStorage;
using BlazorMenu.Constants.Storage;
using BlazorMenu.Services;
using BlazorMenu.Shared.Tabs;
using BlazorMenuCommon.DTOs;
using BlazorMenuModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using R_AuthenticationEnumAndInterface;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorMenu.Authentication
{
    public class BlazorMenuAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly R_ITokenRepository _tokenRepository;
        private readonly ISyncLocalStorageService _localStorageService;
        private readonly R_IMenuService _menuService;
        private readonly NavigationManager _navigationManager;
        private readonly MenuTabSetTool _menuTabSetTool;
        private readonly IClientHelper _clientHelper;

        public BlazorMenuAuthenticationStateProvider(
            R_ITokenRepository tokenRepository,
            ISyncLocalStorageService localStorageService,
            IClientHelper clientHelper,
            R_IMenuService menuService,
            NavigationManager navigationManager,
            MenuTabSetTool menuTabSetTool)
        {
            _tokenRepository = tokenRepository;
            _localStorageService = localStorageService;
            _clientHelper = clientHelper;
            _menuService = menuService;
            _navigationManager = navigationManager;
            _menuTabSetTool = menuTabSetTool;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var loEx = new R_Exception();
            AuthenticationState loState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            try
            {
                var lcSavedToken = _tokenRepository.R_GetToken();

                if (string.IsNullOrWhiteSpace(lcSavedToken))
                    return loState;

                var loUserClaim = GetClaimsFromToken(lcSavedToken);

                _clientHelper.Set_CompanyId(loUserClaim.Where(x => x.Type == "COMPANY_ID").FirstOrDefault().Value);
                _clientHelper.Set_UserId(loUserClaim.Where(x => x.Type == "USER_ID").FirstOrDefault().Value);

                var lcCultureId = _localStorageService.GetItemAsString(StorageConstants.Culture);
                if (!string.IsNullOrWhiteSpace(lcCultureId))
                {
                    var leLoginCulture = R_Culture.R_GetCultureEnum(lcCultureId);

                    _clientHelper.Set_CultureUI(leLoginCulture);
                }
                else
                {
                    _clientHelper.Set_CultureUI(eCulture.English);
                }

                //if (!_navigationManager.Uri.Equals(_navigationManager.BaseUri, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    _navigationManager.NavigateTo(_navigationManager.BaseUri);
                //}

                if (_menuService.MenuAccess == null)
                    await _menuService.SetMenuAccessAsync();

                loState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(loUserClaim, "jwt")));
            }
            catch (Exception)
            {
                await UserLockingFlushAsync();
                return loState;
            }

            loEx.ThrowExceptionIfErrors();

            return loState;
        }

        public async Task MarkUserAsLoggedOut()
        {
            var loEx = new R_Exception();

            try
            {
                await UserLockingFlushAsync();

                var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
                var authState = Task.FromResult(new AuthenticationState(anonymousUser));

                NotifyAuthenticationStateChanged(authState);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task MarkUserAsAuthenticated()
        {
            var authState = Task.FromResult(await GetAuthenticationStateAsync());

            NotifyAuthenticationStateChanged(authState);
        }

        private async Task UserLockingFlushAsync()
        {
            var lcSavedToken = _tokenRepository.R_GetToken();
            var loUserClaim = GetClaimsFromToken(lcSavedToken);

            var loParam = new LoginDTO
            {
                CCOMPANY_ID = loUserClaim.Where(x => x.Type == "COMPANY_ID").FirstOrDefault().Value,
                CUSER_ID = loUserClaim.Where(x => x.Type == "USER_ID").FirstOrDefault().Value
            };

            R_LoginViewModel _loginViewModel = new R_LoginViewModel();
            await _loginViewModel.UserLockingFlushAsync(loParam);

            ClearLocalStorage();
        }

        private void ClearLocalStorage()
        {
            _localStorageService.RemoveItem(StorageConstants.AuthToken);
            _localStorageService.RemoveItem(StorageConstants.Culture);
            _localStorageService.RemoveItem(StorageConstants.TokenId);
        }

        private IEnumerable<Claim> GetClaimsFromToken(string pcToken)
        {
            var loClaims = new List<Claim>();
            var lcPayload = pcToken.Split('.')[1];
            var loJsonBytes = ParseBase64WithoutPadding(lcPayload);
            var loKeyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(loJsonBytes);

            if (loKeyValuePairs != null)
            {
                loKeyValuePairs.TryGetValue("COMPANY_ID", out var lcCompanyId);
                if (lcCompanyId != null)
                {
                    var parsedValue = JsonSerializer.Deserialize<string>((JsonElement)lcCompanyId);
                    loClaims.Add(new Claim("COMPANY_ID", parsedValue));
                }

                loKeyValuePairs.TryGetValue("USER_ID", out var lcUserId);
                if (lcUserId != null)
                {
                    var parsedValue = JsonSerializer.Deserialize<string>((JsonElement)lcUserId);
                    loClaims.Add(new Claim("USER_ID", parsedValue));
                }

                loKeyValuePairs.TryGetValue("USER_ROLE", out var lcUserRole);
                if (lcUserRole != null)
                {
                    var parsedValue = JsonSerializer.Deserialize<string>((JsonElement)lcUserRole);
                    loClaims.Add(new Claim("USER_ROLE", parsedValue));
                }
            }

            return loClaims;
        }

        private byte[] ParseBase64WithoutPadding(string lcPayload)
        {
            lcPayload = lcPayload.Trim().Replace('-', '+').Replace('-', '/');
            var lcBase64 = lcPayload.PadRight(lcPayload.Length + (4 - lcPayload.Length % 4) % 4, '=');

            return Convert.FromBase64String(lcBase64);
        }
    }
}
