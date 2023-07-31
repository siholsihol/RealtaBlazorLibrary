using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.Menu;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTab : R_IMenuTab
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public bool IsActive { get; set; }
        public bool IsInited { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public RenderFragment Body { get; set; }
        public bool Disabled { get; set; }
        public bool Visible { get; set; } = true;
        public bool Closeable { get; set; } = true;
        public string Access { get; set; }
        public string PageTitle { get; set; }

        public Func<Task<bool>> OnCloseMenuTab { get; set; }
    }
}
