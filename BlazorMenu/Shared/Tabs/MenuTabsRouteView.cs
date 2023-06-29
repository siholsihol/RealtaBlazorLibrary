using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Attributes;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Exceptions;
using System.Reflection;

namespace BlazorMenu.Shared.Tabs
{
    public class MenuTabsRouteView : RouteView
    {
        [Inject] public MenuTabSetTool TabSetTool { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var body = CreatePage(RouteData);

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
                    selTab.PageTitle = GetTitleFromPageAttribute(RouteData.PageType);

                    if (isLoad)
                    {
                        selTab.IsInited = true;
                    }
                }
            }
        }

        private RenderFragment CreatePage(RouteData routeData)
        {
            RenderFragment page = builder =>
            {
                builder.OpenComponent(0, routeData.PageType);
                foreach (var routeValue in routeData.RouteValues)
                {
                    builder.AddAttribute(1, routeValue.Key, routeValue.Value);
                }

                if (routeData.PageType.IsSubclassOf(typeof(R_Page)))
                {
                    var loAccess = GetFullFormAccess();
                    builder.AddAttribute(1, "FormAccess", loAccess);
                    builder.AddAttribute(2, "FormModel", R_eFormModel.MainForm);
                }

                builder.CloseComponent();
            };

            return page;
        }

        private string GetTitleFromPageAttribute(Type poPageType)
        {
            var loEx = new R_Exception();
            var lcRtn = "";

            try
            {
                if (!poPageType.IsSubclassOf(typeof(R_Page)))
                    throw new Exception("Type parameter is not inherited from R_Page.");

                var loAttributes = poPageType.GetCustomAttributes(true);

                if (loAttributes.FirstOrDefault(x => x is R_PageAttribute) is R_PageAttribute loPageAttribute && loPageAttribute != null)
                    lcRtn = loPageAttribute.Title;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return lcRtn;
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
