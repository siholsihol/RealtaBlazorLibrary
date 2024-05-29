using BlazorMenu.Services;
using BlazorMenu.Shared.Tabs;
using BlazorMenuCommon.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Exceptions;

namespace BlazorMenu.Shared
{
    public partial class NavMenu
    {
        private bool IconMenuActive { get; set; } = false;

        private bool _collapseNavMenu = true;
        private bool _expandedSubMenu = false;
        private bool _expandedSubNav = false;

        private string NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

        [Parameter] public EventCallback<bool> ShowIconMenu { get; set; }

        private void ToggleNavMenu()
        {
            _collapseNavMenu = !_collapseNavMenu;
        }

        private async Task ToggleIconMenu()
        {
            IconMenuActive = !IconMenuActive;
            await ShowIconMenu.InvokeAsync(IconMenuActive);
        }

        [Inject] public R_IMenuService _menuService { get; set; }
        public List<MenuListDTO> _menuList { get; set; }
        public Dictionary<string, MenuListDTO> _menuIds { get; set; }

        private string _clickedMenu = string.Empty;
        private string _prevClickedMenu = string.Empty;

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
            _expandedSubMenu = false;

            _clickedMenu = poMenu.CMENU_ID;
            if (_prevClickedMenu != _clickedMenu)
            {
                _menuGroupList = _menuList.Where(x => x.CSUB_MENU_TYPE == "G" && x.CMENU_ID == poMenu.CMENU_ID).OrderBy(x => x.IGROUP_INDEX).ToList();
                _expandedSubNav = false;

                _prevClickedMenu = _clickedMenu;
            }

            _expandedSubNav = !_expandedSubNav;
        }

        private string _clickedGroup = string.Empty;
        private string _prevClickedGroup = string.Empty;
        public List<MenuListDTO> _menuProgramList { get; set; }
        public void GetClickedGroup(MenuListDTO poMenu)
        {
            _clickedGroup = poMenu.CMENU_ID + poMenu.CSUB_MENU_ID;

            if (_prevClickedGroup != _clickedGroup)
            {
                _menuProgramList = _menuList.Where(x => x.CSUB_MENU_TYPE == "P" && x.CMENU_ID == poMenu.CMENU_ID && x.CPARENT_SUB_MENU_ID == poMenu.CSUB_MENU_ID).OrderBy(x => x.IFAVORITE_INDEX).ToList();
                _expandedSubMenu = false;

                _prevClickedGroup = _clickedGroup;
            }

            _expandedSubMenu = !_expandedSubMenu;
        }

        [Inject] public MenuTabSetTool TabSetTool { get; set; }

        private void GoTo(MenuListDTO poMenu)
        {
            //TabSetTool.AddTab(poMenu.CSUB_MENU_NAME, poMenu.CSUB_MENU_ID, poMenu.CSUB_MENU_ACCESS);
            TabSetTool.AddTab(poMenu.CSUB_MENU_NAME, poMenu.CSUB_MENU_ID, "A,U,D,P,V");
        }
    }
}
