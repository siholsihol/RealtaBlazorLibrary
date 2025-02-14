﻿using System.Security.Claims;
using System.Text.Json;
using BlazorClientHelper;
using BlazorMenu.Services;
using BlazorMenuCommon.DTOs;
using BlazorMenuModel;
using Microsoft.AspNetCore.Components.Authorization;
using R_AuthenticationEnumAndInterface;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_SecurityPolicyCommon.DTOs;
using R_SecurityPolicyModel;

namespace BlazorMenu.Authentication
{
    public class BlazorMenuAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly R_ITokenRepository _tokenRepository;
        private readonly BlazorMenuLocalStorageService _localStorageService;
        private readonly IClientHelper _clientHelper;

        public BlazorMenuAuthenticationStateProvider(
            R_ITokenRepository tokenRepository,
            BlazorMenuLocalStorageService localStorageService,
            IClientHelper clientHelper)
        {
            _tokenRepository = tokenRepository;
            _localStorageService = localStorageService;
            _clientHelper = clientHelper;
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

                if (_tokenRepository.R_IsTokenExpired())
                {
                    //await _localStorageService.ClearLocalStorageAsync();
                    //return loState;
                }

                var loUserClaim = GetClaimsFromToken(lcSavedToken);

                _clientHelper.Set_CompanyId(loUserClaim.FirstOrDefault(x => x.Type == "COMPANY_ID").Value);
                _clientHelper.Set_UserId(loUserClaim.FirstOrDefault(x => x.Type == "USER_ID").Value);

                var lcCultureId = _localStorageService.GetCulture();
                if (!string.IsNullOrWhiteSpace(lcCultureId))
                {
                    var leLoginCulture = R_Culture.R_GetCultureEnum(lcCultureId);

                    _clientHelper.Set_CultureUI(leLoginCulture);
                }
                else
                {
                    _clientHelper.Set_CultureUI(eCulture.English);
                }

                var loStorageCultureInfo = _localStorageService.GetCultureInfo();

                var loCultureInfoBuilder = new CultureInfoBuilder();
                loCultureInfoBuilder.WithNumberFormatInfo(loStorageCultureInfo["CNUMBER_FORMAT"], Convert.ToInt32(loStorageCultureInfo["IDECIMAL_PLACES"]))
                                    .WithDatePattern(loStorageCultureInfo["CDATE_LONG_FORMAT"], loStorageCultureInfo["CDATE_SHORT_FORMAT"])
                                    .WithTimePattern(loStorageCultureInfo["CTIME_LONG_FORMAT"], loStorageCultureInfo["CTIME_SHORT_FORMAT"]);

                var loCultureInfo = loCultureInfoBuilder.BuildCultureInfo();
                _clientHelper.Set_Culture(loCultureInfo.NumberFormat, loCultureInfo.DateTimeFormat);

                var lcReportCulture = await _localStorageService.GetCultureReportAsync();
                _clientHelper.Set_ReportCulture(lcReportCulture);

                _clientHelper.Set_ProgramId("");

                loState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(loUserClaim, "jwt")));
            }
            catch (Exception)
            {
                await _localStorageService.ClearLocalStorageAsync();
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
                var lcSavedToken = _tokenRepository.R_GetToken();
                var loUserClaim = GetClaimsFromToken(lcSavedToken);

                var loSecurityModel = new R_SecurityModel();
                await loSecurityModel.R_SecurityPolicyLogoutAsync(new SecurityPolicyLogoutParameterDTO
                {
                    CCOMPANY_ID = loUserClaim.FirstOrDefault(x => x.Type == "COMPANY_ID").Value,
                    CUSER_ID = loUserClaim.FirstOrDefault(x => x.Type == "USER_ID").Value
                });

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
            var loEx = new R_Exception();

            try
            {
                var lcSavedToken = _tokenRepository.R_GetToken();
                var loUserClaim = GetClaimsFromToken(lcSavedToken);

                var loParam = new UserLockingFlushParameterDTO
                {
                    CCOMPANY_ID = loUserClaim.FirstOrDefault(x => x.Type == "COMPANY_ID").Value,
                    CUSER_ID = loUserClaim.FirstOrDefault(x => x.Type == "USER_ID").Value
                };

                var loLoginViewModel = new R_LoginViewModel();
                await loLoginViewModel.UserLockingFlushAsync(loParam);

                await _localStorageService.ClearLocalStorageAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private static IEnumerable<Claim> GetClaimsFromToken(string pcToken)
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

        private static byte[] ParseBase64WithoutPadding(string lcPayload)
        {
            lcPayload = lcPayload.Trim().Replace('-', '+').Replace('-', '/');
            var lcBase64 = lcPayload.PadRight(lcPayload.Length + (4 - lcPayload.Length % 4) % 4, '=');

            return Convert.FromBase64String(lcBase64);
        }
    }
}
