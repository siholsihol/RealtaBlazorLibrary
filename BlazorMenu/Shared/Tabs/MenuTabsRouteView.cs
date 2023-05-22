using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Enums;
using System.Reflection;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTabsRouteView : RouteView
    {
        [Inject]
        public MenuTabSetTool TabSetTool { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var body = CreateBody(RouteData, NavigationManager.Uri);

            RenderContentInDefaultLayout(builder, body, true);
        }

        private void RenderContentInDefaultLayout(RenderTreeBuilder builder, RenderFragment body, bool isLoad = false)
        {
            var layoutType = RouteData.PageType.GetCustomAttribute<LayoutAttribute>()?.LayoutType ?? DefaultLayout;
            builder.OpenComponent<CascadingValue<MenuTabsRouteView>>(0);
            builder.AddAttribute(1, "Name", "RouteView");
            builder.AddAttribute(2, "Value", this);
            builder.AddAttribute(3, "ChildContent", (RenderFragment)(b =>
            {
                b.OpenComponent(20, layoutType);
                b.AddAttribute(21, "Body", body);
                b.CloseComponent();
            }));
            builder.CloseComponent();

            var url = "/" + NavigationManager.ToBaseRelativePath(NavigationManager.Uri);

            if (!url.Equals("/"))
            {
                var selTab = TabSetTool.Tabs.FirstOrDefault(m => !m.IsInited);

                if (selTab == null)
                {
                    if (TabSetTool.Tabs.Count == 0)
                    {
                        TabSetTool.Tabs.Add(new MenuTab
                        {
                            Body = body,
                            Url = url,
                            IsInited = isLoad,
                            IsActive = true,
                            Title = url.Equals("/") ? "Home" : ""
                        });
                    }
                }
                else
                {
                    selTab.Body = body;
                    selTab.IsActive = true;
                    if (isLoad)
                    {
                        selTab.IsInited = true;
                    }
                }
            }
        }

        private RenderFragment CreateBody(RouteData routeData, string uri)
        {
            return builder =>
            {
                builder.OpenComponent(0, routeData.PageType);
                foreach (var routeValue in routeData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                    if (routeData.PageType.IsSubclassOf(typeof(R_Page)))
                    {
                        var loAccess = GetFullFormAccess();
                        builder.AddAttribute(2, "R_FormAccess", loAccess);
                    }
                }
                builder.CloseComponent();
            };
        }

        private R_eFormAccess[] GetFullFormAccess()
        {
            R_eFormAccess[] loFormAccess =
            {
                R_eFormAccess.Add,
                R_eFormAccess.Update,
                R_eFormAccess.Delete,
                R_eFormAccess.Print,
                R_eFormAccess.View
            };

            return loFormAccess;
        }
    }
}
