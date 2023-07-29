using BlazorClientHelper;
using BlazorMenuCommon.DTOs;
using BlazorMenuCommon.Requests;
using BlazorMenuModel;
using R_BlazorFrontEnd.Configurations;
using R_BlazorFrontEnd.Exceptions;

namespace BlazorMenu.Services
{
    public class R_MenuService : R_IMenuService
    {
        private readonly IClientHelper _clientHelper;
        private R_MenuModel _menuModel = null;

        public R_MenuService(IClientHelper clientHelper)
        {
            _clientHelper = clientHelper;
            _menuModel = new R_MenuModel();
        }

        public async Task<List<MenuListDTO>> GetMenuAsync()
        {
            var loEx = new R_Exception();
            List<MenuListDTO> loResult = null;

            try
            {
                var loParam = new GetMenuRequest
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CUSER_ID = _clientHelper.UserId,
                    CLANGUAGE_ID = "en",
                    CMENU_ID = "",
                    CSUB_MENU_ID = "",
                    ILEVEL = 1,
                    CMODUL_ID = R_FrontConfig.R_GetConfigAsString("R_Md")
                };

                var loMenuList = await _menuModel.GetMenuAsync(loParam);

                loResult = loMenuList.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
    }
}
