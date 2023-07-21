using Telerik.SvgIcons;

namespace BlazorMenu.Shared.Drawer
{
    public class DrawerMenuItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public ISvgIcon Icon { get; set; } = SvgIcon.Categorize;
        public bool Expanded { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public List<DrawerMenuItem> Children { get; set; }
    }
}
