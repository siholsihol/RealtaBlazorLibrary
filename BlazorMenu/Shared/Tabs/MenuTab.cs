using Microsoft.AspNetCore.Components;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTab
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
    }
}
