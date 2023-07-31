using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Interfaces;
using R_BlazorFrontEnd.Controls.Menu;
using R_BlazorFrontEnd.Controls.Menu.Tab;

namespace BlazorMenu.Shared
{
    public partial class MainBody : ComponentBase, R_IMainBody
    {
        [CascadingParameter(Name = "currentTabMenu")] public MenuTab CurrentTab { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public List<R_TabProgram> Tabs { get; set; } = new();
        public event AsyncEventHandler<R_TabProgram> OnInternalTabChangingAsync;

        private R_Tabs _tabRef;

        protected override async Task OnInitializedAsync()
        {
            if (CurrentTab != null)
                await R_AddTab(
                    string.IsNullOrWhiteSpace(CurrentTab.PageTitle) ? CurrentTab.Title : CurrentTab.PageTitle,
                    CurrentTab.Body,
                    true,
                    null,
                    null,
                    null);
        }

        public async Task R_AddTab(
            string pcTitle,
            RenderFragment poBody,
            bool plSetActive,
            R_Page poParentPage,
            R_IDetail poDetailButton,
            R_PredefinedDock poPredefinedDock)
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
                    DetailButton = poDetailButton,
                    PredefinedDock = poPredefinedDock,
                    HeaderEnabled = poPredefinedDock != null && !poPredefinedDock.Enabled ? false : true
                };

                if (poPredefinedDock is not null)
                {
                    poPredefinedDock.EnabledChanged = () => _tabRef.SetEnableTab(loNewTab.Id.ToString());
                }

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

            if (poRemovedTab.Closing != null)
                eventArgs.Cancel = await poRemovedTab.Closing();
        }

        private async Task OnTabRemoved(R_Tab tab)
        {
            var poRemovedTab = GetTabById(Guid.Parse(tab.Id));

            object loDetailResult = null;
            if (poRemovedTab.Close != null)
                loDetailResult = await poRemovedTab.Close();

            Tabs.Remove(poRemovedTab);

            if (poRemovedTab.DetailButton != null)
            {
                if (poRemovedTab.DetailButton.R_After_Open_Detail.HasDelegate)
                {
                    var loEventArgs = new R_AfterOpenDetailEventArgs(loDetailResult);
                    await poRemovedTab.DetailButton.R_After_Open_Detail.InvokeAsync(loEventArgs);
                }
            }
        }

        private void OnActiveTabChanged(R_Tab tab)
        {
            Tabs.ForEach(x => { x.IsActive = false; });

            var poActiveTab = GetTabById(Guid.Parse(tab.Id));

            poActiveTab.IsActive = true;
        }

        private async Task OnActiveTabChanging(R_ActiveTabChangingEventArgs eventArgs)
        {
            var poOldTab = GetTabById(Guid.Parse(eventArgs.OldTab.Id));
            var poNewTab = GetTabById(Guid.Parse(eventArgs.NewTab.Id));

            if (poNewTab is not null)
            {
                if (OnInternalTabChangingAsync is not null)
                    await OnInternalTabChangingAsync(this, poNewTab);
            }

            if (poOldTab is not null && poOldTab.PredefinedDock is not null)
            {
                object loDetailResult = null;
                if (poOldTab.Close != null)
                    loDetailResult = await poOldTab.Close();

                if (poOldTab.PredefinedDock.R_AfterOpenPredefinedDock.HasDelegate)
                {
                    var loEventArgs = new R_AfterOpenPredefinedDockEventArgs(loDetailResult);
                    await poOldTab.PredefinedDock.R_AfterOpenPredefinedDock.InvokeAsync(loEventArgs);
                }
            }
        }

        public R_TabProgram GetTabById(Guid tabId)
        {
            return Tabs.FirstOrDefault(x => x.Id == tabId);
        }
    }
}
