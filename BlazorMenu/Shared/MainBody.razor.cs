using BlazorMenu.Shared.Tabs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Menu;
using R_BlazorFrontEnd.Controls.Menu.Tab;

namespace BlazorMenu.Shared
{
    public partial class MainBody : ComponentBase, R_IMainBody
    {
        [Parameter] public RenderFragment ChildContent { get; set; }

        public List<R_TabProgram> Tabs { get; set; } = new();

        [CascadingParameter(Name = "currentTabMenu")]
        public MenuTab CurrentTab { get; set; }

        private R_Tabs _tabRef;

        protected override async Task OnInitializedAsync()
        {
            if (CurrentTab != null)
                await R_AddTab(
                    CurrentTab.Title,
                    CurrentTab.Body,
                    true,
                    false,
                    null,
                    R_eFormModel.MainForm,
                    CurrentTab.Access,
                    null,
                    null);
        }

        public async Task R_AddTab(
            string pcTitle,
            RenderFragment poBody,
            bool plSetActive,
            bool plCloseable,
            object poParameter,
            R_eFormModel peFormModel,
            string pcFormAccess,
            R_Page poParentPage,
            R_Detail poDetailButton)
        {
            R_TabProgram loNewTab = null;
            var selTab = Tabs.FirstOrDefault(m => m.Title == pcTitle || string.IsNullOrEmpty(m.Title));
            if (selTab == null)
            {
                loNewTab = new R_TabProgram
                {
                    Title = pcTitle,
                    IsActive = plSetActive,
                    Body = poBody,
                    Closeable = plCloseable,
                    Parameter = poParameter,
                    FormModel = peFormModel,
                    Access = pcFormAccess,
                    ParentPage = poParentPage,
                    DetailButton = poDetailButton
                };

                Tabs.Add(loNewTab);
            }
            else
            {
                loNewTab = selTab;
            }

            if (loNewTab.FormModel == R_eFormModel.Detail ||
                loNewTab.FormModel == R_eFormModel.Predefine)
            {
                if (selTab != null)
                {
                    var loTab = _tabRef.GetTabById(selTab.Id.ToString());
                    await _tabRef.SetActiveTabAsync(loTab);
                }
                else
                    _tabRef.NotifyStateHasChanged();
            }
        }

        private async Task OnTabRemoving(R_TabRemovingEventArgs eventArgs)
        {
            var poRemovedTab = Tabs.FirstOrDefault(x => x.Id == Guid.Parse(eventArgs.Tab.Id));

            if (poRemovedTab.Close != null)
            {
                var llResult = await poRemovedTab.Close(true, null);

                eventArgs.Cancel = !llResult;
            }
        }

        private void OnTabRemoved(R_Tab tab)
        {
            var poRemovedTab = Tabs.FirstOrDefault(x => x.Id == Guid.Parse(tab.Id));

            Tabs.Remove(poRemovedTab);

            if (poRemovedTab.FormModel == R_eFormModel.Detail && poRemovedTab.DetailButton != null)
            {
                if (poRemovedTab.DetailButton.R_After_Open_Detail.HasDelegate)
                    poRemovedTab.DetailButton.R_After_Open_Detail.InvokeAsync();
            }
        }

        private void OnActiveTabChanged(R_Tab tab)
        {
            Tabs.ForEach(x => { x.IsActive = false; });

            var poActiveTab = Tabs.FirstOrDefault(x => x.Id == Guid.Parse(tab.Id));

            poActiveTab.IsActive = true;
        }
    }
}
