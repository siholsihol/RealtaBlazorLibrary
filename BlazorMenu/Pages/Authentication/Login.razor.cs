using BlazorClientHelper;
using Blazored.LocalStorage;
using BlazorMenu.Authentication;
using BlazorMenu.Constants.Storage;
using BlazorMenu.Services;
using BlazorMenu.Shared.Tabs;
using BlazorMenuModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using R_AuthenticationEnumAndInterface;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Extensions;
using R_CommonFrontBackAPI;
using R_CrossPlatformSecurity;
using R_SecurityPolicyCommon;
using R_SecurityPolicyFront;

namespace BlazorMenu.Pages.Authentication
{
    public partial class Login
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_ITokenRepository _tokenRepository { get; set; }
        [Inject] private ILocalStorageService _localStorageService { get; set; }
        [Inject] private R_IMenuService _menuService { get; set; }
        //[Inject] private R_ILoginService _loginService { get; set; }
        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] public R_MessageBoxService R_MessageBox { get; set; }

        private LoginModel _loginModel = new LoginModel();

        R_SecurityPolicyClient loClientWrapper = new R_SecurityPolicyClient();
        [Inject] private R_ISymmetricJSProvider _encryptProvider { get; set; }
        [Inject] private MenuTabSetTool MenuTabSetTool { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var state = await _stateProvider.GetAuthenticationStateAsync();
                if (state.User.Identity.IsAuthenticated)
                    _navigationManager.NavigateTo("/");

                var loPolicyParameter = await loClientWrapper.R_GetSecurityPolicyParameterAsync();

                _loginModel.CompanyId = "rcd";
                _loginModel.UserId = "cp";
                _loginModel.Password = "cp";
            }
            catch (R_Exception rex)
            {
                loEx.Add(rex);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
                await R_MessageBox.Show("Error", loEx.ErrorList[0].ErrDescp, R_eMessageBoxButtonType.OK);
        }

        private async Task ValidateUser()
        {
            var loEx = new R_Exception();

            try
            {
                _clientHelper.Set_ComputerId();
                _clientHelper.Set_CompanyId(_loginModel.CompanyId);

                var lcEncryptedPassword = await _encryptProvider.TextEncrypt(_loginModel.Password, _loginModel.UserId);

                var loPolicyLogin = await loClientWrapper.R_SecurityPolicyLogonAsync
                    (
                        new R_SecurityPolicyDTO
                        {
                            CCOMPANY_ID = _loginModel.CompanyId,
                            CUSER_ID = _loginModel.UserId.ToLower(),
                            CUSER_PASSWORD = lcEncryptedPassword
                        }
                    );

                if (loPolicyLogin.Data != null)
                {
                    _tokenRepository.R_SetToken(loPolicyLogin.Data.CTOKEN);

                    await _localStorageService.SetItemAsStringAsync(StorageConstants.TokenId, loPolicyLogin.Data.CTOKEN_ID);

                    R_LoginViewModel _loginViewModel = new R_LoginViewModel();
                    var loLogin = await _loginViewModel.LoginAsync(_loginModel);

                    _clientHelper.Set_UserId(loLogin.CUSER_ID);
                    _clientHelper.Set_UserName(loLogin.CUSER_NAME);

                    await _menuService.SetMenuAccessAsync();

                    if (!string.IsNullOrWhiteSpace(loLogin.CCULTURE_ID))
                    {
                        var leLoginCulture = R_Culture.R_GetCultureEnum(loLogin.CCULTURE_ID);

                        _clientHelper.Set_CultureUI(leLoginCulture);
                    }
                    else
                    {
                        _clientHelper.Set_CultureUI(eCulture.English);
                    }

                    var loNumberInfo = Thread.CurrentThread.CurrentUICulture.NumberFormat;
                    if (!string.IsNullOrWhiteSpace(loLogin.CNUMBER_FORMAT))
                    {
                        loNumberInfo.NumberDecimalSeparator = loLogin.CNUMBER_FORMAT;
                        loNumberInfo.NumberGroupSeparator = loLogin.CNUMBER_FORMAT == "," ? "." : ",";
                        loNumberInfo.CurrencyDecimalSeparator = loLogin.CNUMBER_FORMAT;
                        loNumberInfo.CurrencyGroupSeparator = loLogin.CNUMBER_FORMAT == "," ? "." : ",";
                        loNumberInfo.PercentDecimalSeparator = loLogin.CNUMBER_FORMAT;
                        loNumberInfo.PercentGroupSeparator = loLogin.CNUMBER_FORMAT == "," ? "." : ",";
                    }
                    else
                    {
                        loNumberInfo = new System.Globalization.NumberFormatInfo
                        {
                            NumberDecimalSeparator = ",",
                            NumberGroupSeparator = "."
                        };
                    }

                    loNumberInfo.NumberDecimalDigits = loLogin.IDECIMAL_PLACES;

                    R_eRoundingMethod leRounding = R_EnumExtensions.ToEnum<R_eRoundingMethod>(loLogin.CROUNDING_METHOD);

                    var loDateInfo = Thread.CurrentThread.CurrentUICulture.DateTimeFormat;
                    loDateInfo.LongDatePattern = !string.IsNullOrWhiteSpace(loLogin.CDATE_LONG_FORMAT) ? loLogin.CDATE_LONG_FORMAT : "MMMM d, yyyy";
                    loDateInfo.ShortDatePattern = !string.IsNullOrWhiteSpace(loLogin.CDATE_SHORT_FORMAT) ? loLogin.CDATE_SHORT_FORMAT : "M/d/yy";
                    loDateInfo.LongTimePattern = !string.IsNullOrWhiteSpace(loLogin.CTIME_LONG_FORMAT) ? loLogin.CTIME_LONG_FORMAT : "hh:mm:ss tt";
                    loDateInfo.ShortTimePattern = !string.IsNullOrWhiteSpace(loLogin.CTIME_SHORT_FORMAT) ? loLogin.CTIME_SHORT_FORMAT : "hh:mm tt";

                    _clientHelper.Set_Culture(loNumberInfo, loDateInfo);

                    await _localStorageService.SetItemAsStringAsync(StorageConstants.Culture, loLogin.CCULTURE_ID);
                    if (!loLogin.CCULTURE_ID.Equals("en", StringComparison.InvariantCultureIgnoreCase))
                        _navigationManager.NavigateTo(_navigationManager.Uri, true);

                    MenuTabSetTool.Tabs.Clear();

                    await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated();
                }
                else
                {
                    throw new Exception("Invalid username or password");
                }
            }
            catch (R_Exception rex)
            {
                loEx.Add(rex);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            if (loEx.HasError)
            {
                await R_MessageBox.Show("Error", loEx.ErrorList[0].ErrDescp, R_eMessageBoxButtonType.OK);
                _tokenRepository.R_SetToken("");
            }
        }
    }
}
