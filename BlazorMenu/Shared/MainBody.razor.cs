using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Menu;
using R_BlazorFrontEnd.Controls.Menu.Tab;

namespace BlazorMenu.Shared
{
    public partial class MainBody : ComponentBase, R_IMainBody
    {
        [CascadingParameter(Name = "currentTabMenu")] public MenuTab CurrentTab { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public List<R_TabProgram> Tabs { get; set; } = new();

        private R_Tabs _tabRef;

        protected override async Task OnInitializedAsync()
        {
            if (CurrentTab != null)
                await R_AddTab(
                    CurrentTab.Title,
                    CurrentTab.Body,
                    true,
                    null,
                    null);
        }

        public async Task R_AddTab(
            string pcTitle,
            RenderFragment poBody,
            bool plSetActive,
            R_Page poParentPage,
            R_Detail poDetailButton)
        {
            R_TabProgram loNewTab = null;

            var selTab = Tabs.FirstOrDefault(m => m.DetailButton != null && m.DetailButton.Id == poDetailButton.Id);
            if (selTab == null)
            {
                loNewTab = new R_TabProgram
                {
                    Title = pcTitle,
                    IsActive = plSetActive,
                    Body = poBody,
                    ParentPage = poParentPage,
                    DetailButton = poDetailButton
                };

                Tabs.Add(loNewTab);
            }
            else
            {
                loNewTab = selTab;
            }

            if (selTab != null)
            {
                var loTab = _tabRef.GetTabById(selTab.Id.ToString());
                await _tabRef.SetActiveTabAsync(loTab);
            }
            else
                _tabRef?.NotifyStateHasChanged();
        }

        private async Task OnTabRemoving(R_TabRemovingEventArgs eventArgs)
        {
            var poRemovedTab = GetTabById(Guid.Parse(eventArgs.Tab.Id));

            if (poRemovedTab.Close != null)
            {
                var llResult = await poRemovedTab.Close(true, null);

                eventArgs.Cancel = !llResult;
            }
        }

        private void OnTabRemoved(R_Tab tab)
        {
            var poRemovedTab = GetTabById(Guid.Parse(tab.Id));

            Tabs.Remove(poRemovedTab);

            if (poRemovedTab.DetailButton != null)
            {
                if (poRemovedTab.DetailButton.R_After_Open_Detail.HasDelegate)
                    poRemovedTab.DetailButton.R_After_Open_Detail.InvokeAsync();
            }
        }

        private void OnActiveTabChanged(R_Tab tab)
        {
            Tabs.ForEach(x => { x.IsActive = false; });

            var poActiveTab = GetTabById(Guid.Parse(tab.Id));

            poActiveTab.IsActive = true;
        }

        private void OnActiveTabChanging(R_ActiveTabChangingEventArgs eventArgs)
        {

        }

        public R_TabProgram GetTabById(Guid tabId)
        {
            return Tabs.FirstOrDefault(x => x.Id == tabId);
        }
    }
}
