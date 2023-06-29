using Microsoft.AspNetCore.Components;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTabSetTool
    {
        private readonly NavigationManager _navigationManager;

        public MenuTabSetTool(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public List<MenuTab> Tabs { get; set; } = new();
        public int ActiveTabIndex = 0;

        public void AddTab(string title, string url, string pcAccess = "")
        {
            Tabs.ForEach(x =>
            {
                x.IsActive = false;
            });

            MenuTab loNewTab = null;
            var selTab = Tabs.FirstOrDefault(m => m.Url == url && (m.Title == title || string.IsNullOrEmpty(m.Title)));
            if (selTab == null)
            {
                var lcAccess = string.IsNullOrWhiteSpace(pcAccess) ? "V" : pcAccess;

                loNewTab = new MenuTab
                {
                    Url = url,
                    Title = title,
                    IsActive = true,
                    Access = lcAccess
                };

                Tabs.Add(loNewTab);
            }
            else
            {
                if (string.IsNullOrEmpty(selTab.Title))
                    selTab.Title = title;

                selTab.IsActive = true;
            }

            _navigationManager.NavigateTo(url);
        }
    }
}
