using BlazorClientHelper;
using BlazorMenu.Authentication;
using BlazorMenu.Services;
using BlazorMenu.Shared.Tabs;
using BlazorMenuModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using R_AuthenticationEnumAndInterface;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls.Notification;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_CrossPlatformSecurity;

namespace BlazorMenu.Pages.Authentication
{
    public partial class Login
    {
        [Inject] private AuthenticationStateProvider _stateProvider { get; set; }
        [Inject] private R_ITokenRepository _tokenRepository { get; set; }
        //[Inject] private ILocalStorageService _localStorageService { get; set; }
        [Inject] private BlazorMenuLocalStorageService _localStorageService { get; set; }
        //[Inject] private R_IMenuService _menuService { get; set; }
        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] public R_MessageBoxService R_MessageBox { get; set; }
        [Inject] private R_ISymmetricJSProvider _encryptProvider { get; set; }
        [Inject] private MenuTabSetTool MenuTabSetTool { get; set; }
        [Inject] private R_NotificationService _notificationService { get; set; }

        //private LoginModel _loginModel = new LoginModel();
        //private R_SecurityModel loClientWrapper = new R_SecurityModel();
        private R_LoginViewModel _loginVM = new R_LoginViewModel();

        protected override async Task OnParametersSetAsync()
        {
            var loEx = new R_Exception();

            try
            {
                var state = await _stateProvider.GetAuthenticationStateAsync();
                if (state.User.Identity.IsAuthenticated)
                    _navigationManager.NavigateTo("/");

                await _loginVM.R_GetSecurityPolicyParameterAsync();

                //_loginVM.LoginModel.CompanyId = "001";
                //_loginVM.LoginModel.UserId = "cp";
                //_loginVM.LoginModel.Password = "cp";
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
                _notificationService.Error(loEx.ErrorList[0].ErrDescp);
        }

        private async Task ValidateUser()
        {
            var loEx = new R_Exception();

            try
            {
                _clientHelper.Set_ComputerId();
                _clientHelper.Set_CompanyId(_loginVM.LoginModel.CompanyId);

                var lcEncryptedPassword = await _encryptProvider.TextEncrypt(_loginVM.LoginModel.Password, _loginVM.LoginModel.UserId);

                //var loPolicyLogin = await loClientWrapper.R_SecurityPolicyLogonAsync
                //    (
                //        new SecurityPolicyLogonParameterDTO
                //        {
                //            CCOMPANY_ID = _loginModel.CompanyId,
                //            CUSER_ID = _loginModel.UserId.ToLower(),
                //            CUSER_PASSWORD = lcEncryptedPassword
                //        }
                //    );

                await _loginVM.LoginAsync(lcEncryptedPassword);

                _tokenRepository.R_SetToken(_loginVM.LoginResult.CTOKEN);
                _tokenRepository.R_SetRefreshToken(_loginVM.LoginResult.CREFRESH_TOKEN);

                _clientHelper.Set_UserId(_loginVM.LoginResult.CUSER_ID);
                _clientHelper.Set_UserName(_loginVM.LoginResult.CUSER_NAME);

                if (!string.IsNullOrWhiteSpace(_loginVM.LoginResult.CCULTURE_ID))
                {
                    var leLoginCulture = R_Culture.R_GetCultureEnum(_loginVM.LoginResult.CCULTURE_ID);

                    _clientHelper.Set_CultureUI(leLoginCulture);
                }
                else
                    _clientHelper.Set_CultureUI(eCulture.English);

                var loCultureInfoBuilder = new CultureInfoBuilder();
                loCultureInfoBuilder.WithNumberFormatInfo(_loginVM.LoginResult.CNUMBER_FORMAT, _loginVM.LoginResult.IDECIMAL_PLACES)
                                    .WithDatePattern(_loginVM.LoginResult.CDATE_LONG_FORMAT, _loginVM.LoginResult.CDATE_SHORT_FORMAT)
                                    .WithTimePattern(_loginVM.LoginResult.CTIME_LONG_FORMAT, _loginVM.LoginResult.CTIME_SHORT_FORMAT);

                var loCultureInfo = loCultureInfoBuilder.BuildCultureInfo();

                _clientHelper.Set_Culture(loCultureInfo.NumberFormat, loCultureInfo.DateTimeFormat);

                await _localStorageService.SetCultureAsync(_loginVM.LoginResult.CCULTURE_ID);

                var loDictCulture = new Dictionary<string, string>
                    {
                        { "CNUMBER_FORMAT", _loginVM.LoginResult.CNUMBER_FORMAT },
                        { "IDECIMAL_PLACES", _loginVM.LoginResult.IDECIMAL_PLACES.ToString() },
                        { "CDATE_LONG_FORMAT", _loginVM.LoginResult.CDATE_LONG_FORMAT },
                        { "CDATE_SHORT_FORMAT", _loginVM.LoginResult.CDATE_SHORT_FORMAT },
                        { "CTIME_LONG_FORMAT", _loginVM.LoginResult.CTIME_LONG_FORMAT },
                        { "CTIME_SHORT_FORMAT", _loginVM.LoginResult.CTIME_SHORT_FORMAT }
                    };
                await _localStorageService.SetCultureInfoAsync(loDictCulture);

                if (!_loginVM.LoginResult.CCULTURE_ID.Equals("en", StringComparison.InvariantCultureIgnoreCase))
                    _navigationManager.NavigateTo(_navigationManager.Uri, true);

                MenuTabSetTool.Tabs.Clear();

                await ((BlazorMenuAuthenticationStateProvider)_stateProvider).MarkUserAsAuthenticated();
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
                _notificationService.Error(loEx.ErrorList[0].ErrDescp);

                _tokenRepository.R_SetToken("");
            }
        }
    }
}
