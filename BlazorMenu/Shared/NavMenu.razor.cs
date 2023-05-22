using BlazorMenu.Services;
using BlazorMenu.Shared.Tabs;
using BlazorMenuCommon.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Exceptions;

namespace BlazorMenu.Shared
{
    public partial class NavMenu
    {
        private bool collapseNavMenu = true;
        private bool expandedSubMenu = false;
        private bool expandedSubNav = false;

        private string NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        [Inject] public R_IMenuService _menuService { get; set; }
        public List<MenuListDTO> _menuList { get; set; }
        public Dictionary<string, MenuListDTO> _menuIds { get; set; }

        private string _clickedMenu = "";
        private string _prevClickedMenu = "";

        protected override async Task OnParametersSetAsync()
        {
            var loEx = new R_Exception();

            try
            {
                _menuList = await _menuService.GetMenuAsync();

                _menuIds = _menuList.Where(x => x.CMENU_ID != "FAV")
                    .GroupBy(x => x.CMENU_ID)
                    .Select(x => x.First()).ToDictionary(x => x.CMENU_ID, x => x);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public List<MenuListDTO> _menuGroupList { get; set; }
        public void GetClicked(MenuListDTO poMenu)
        {
            _clickedMenu = poMenu.CMENU_ID;
            if (_prevClickedMenu != _clickedMenu)
            {
                _menuGroupList = _menuList.Where(x => x.CSUB_MENU_TYPE == "G" && x.CMENU_ID == poMenu.CMENU_ID).OrderBy(x => x.IGROUP_INDEX).ToList();
                expandedSubNav = false;

                _prevClickedMenu = _clickedMenu;
            }

            expandedSubNav = !expandedSubNav;
        }

        private string _clickedGroup = "";
        private string _prevClickedGroup = "";
        public List<MenuListDTO> _menuProgramList { get; set; }
        public void GetClickedGroup(MenuListDTO poMenu)
        {
            _clickedGroup = poMenu.CSUB_MENU_ID;

            if (_prevClickedGroup != _clickedGroup)
            {
                _menuProgramList = _menuList.Where(x => x.CSUB_MENU_TYPE == "P" && x.CMENU_ID == poMenu.CMENU_ID && x.CPARENT_SUB_MENU_ID == poMenu.CSUB_MENU_ID).OrderBy(x => x.IFAVORITE_INDEX).ToList();
                expandedSubMenu = false;

                _prevClickedGroup = _clickedGroup;
            }

            expandedSubMenu = !expandedSubMenu;
        }

        [Inject] public MenuTabSetTool TabSetTool { get; set; }

        private void GoTo(MenuListDTO poMenu)
        {
            //TabSetTool.AddTab(poMenu.CSUB_MENU_NAME, poMenu.CSUB_MENU_ID, poMenu.CSUB_MENU_ACCESS);
            TabSetTool.AddTab(poMenu.CSUB_MENU_NAME, poMenu.CSUB_MENU_ID, "A,U,D,P,V");
        }
    }
}
