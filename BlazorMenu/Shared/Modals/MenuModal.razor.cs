using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls.Base;
using R_BlazorFrontEnd.Controls.Helpers;

namespace BlazorMenu.Shared.Modals
{
    public partial class MenuModal : BaseComponent
    {
        #region Properties

        [Parameter] public string Title { get; set; }
        [Parameter] public string Message { get; set; }
        [Parameter] public bool ShowCloseButton { get; set; } = true;
        [Parameter] public RenderFragment HeaderTemplate { get; set; }
        [Parameter] public RenderFragment BodyTemplate { get; set; }
        [Parameter] public RenderFragment FooterTemplate { get; set; }
        [Parameter] public string HeaderCssClass { get; set; }
        [Parameter] public string BodyCssClass { get; set; }
        [Parameter] public string FooterCssClass { get; set; }
        [Parameter] public bool UseStaticBackdrop { get; set; } = true;
        [Parameter] public bool CloseOnEscape { get; set; } = true;

        //[Inject] private MenuModalService ModalService { get; set; }
        [Parameter] public EventCallback OnShowing { get; set; }
        [Parameter] public EventCallback OnShown { get; set; }
        [Parameter] public EventCallback OnHiding { get; set; }
        [Parameter] public EventCallback OnHidden { get; set; }
        [Parameter] public EventCallback OnHidePrevented { get; set; }

        public override string Id { get; set; } = IdGeneratorHelper.Generate("menumodal");

        #endregion

        #region Members

        protected string ClassNames =>
            new CssBuilder(Class)
            .AddClass("modal")
            .AddClass("fade")
            .Build();

        protected string StyleNames =>
            new StyleBuilder(Style)
            .Build();

        private bool isVisible;
        private Type childComponent;
        private Dictionary<string, object> parameters;
        private DotNetObjectReference<MenuModal> objRef;

        #endregion

        #region Methods

        protected override async Task OnInitializedAsync()
        {
            //if (ModalService is not null)
            //    ModalService.OnShow += OnShowAsync;

            objRef ??= DotNetObjectReference.Create(this);
            await base.OnInitializedAsync();

            ExecuteAfterRender(async () => { await JS.InvokeVoidAsync("blazorMenuBootstrap.modal.initialize", Id, UseStaticBackdrop, CloseOnEscape, objRef); });
        }

        private Task OnShowAsync()
        {
            //if (modalOption is null)
            //    throw new ArgumentNullException(nameof(modalOption));

            //modalType = modalOption.Type;

            //Size = modalOption.Size;

            //IsVerticallyCentered = modalOption.IsVerticallyCentered;

            //showFooterButton = modalOption.ShowFooterButton;
            //if (showFooterButton)
            //{
            //    footerButtonColor = modalOption.FooterButtonColor;
            //    footerButtonCSSClass = modalOption.FooterButtonCSSClass;
            //    footerButtonText = modalOption.FooterButtonText;
            //    FooterCssClass = "border-top-0";
            //}

            return ShowAsync(title: null, message: null, type: null, parameters: null);
        }

        public async Task ShowAsync() => await ShowAsync(title: null, message: null, type: null, parameters: null);

        public async Task ShowAsync<T>(string title = null, string message = null, Dictionary<string, object> parameters = null) => await ShowAsync(title: title, message: message, type: typeof(T), parameters: parameters);

        private async Task ShowAsync(string title, string message, Type type, Dictionary<string, object> parameters)
        {
            isVisible = true;

            if (!string.IsNullOrWhiteSpace(title))
                Title = title;

            if (!string.IsNullOrWhiteSpace(message))
                Message = message;

            childComponent = type;
            this.parameters = parameters;

            await InvokeAsync(StateHasChanged);

            await JS.InvokeVoidAsync("blazorMenuBootstrap.modal.show", Id);
        }

        public async Task HideAsync()
        {
            isVisible = false;
            await JS.InvokeVoidAsync("blazorMenuBootstrap.modal.hide", Id);
        }

        [JSInvokable] public async Task bsShowModal() => await OnShowing.InvokeAsync();
        [JSInvokable] public async Task bsShownModal() => await OnShown.InvokeAsync();
        [JSInvokable] public async Task bsHideModal() => await OnHiding.InvokeAsync();
        [JSInvokable] public async Task bsHiddenModal() => await OnHidden.InvokeAsync();
        [JSInvokable] public async Task bsHidePreventedModal() => await OnHidePrevented.InvokeAsync();

        protected override async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                ExecuteAfterRender(async () => { await JS.InvokeVoidAsync("blazorMenuBootstrap.modal.dispose", Id); });
                objRef?.Dispose();

                //if (ModalService is not null && IsServiceModal)
                //    ModalService.OnShow -= OnShowAsync;
            }

            await base.DisposeAsync(disposing);
        }

        #endregion
    }
}
