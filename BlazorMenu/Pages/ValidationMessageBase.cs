using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using R_BlazorFrontEnd.Controls.Helpers;
using System.Linq.Expressions;

namespace BlazorMenu.Pages
{
    public class ValidationMessageBase<TValue> : ComponentBase, IDisposable
    {
        private FieldIdentifier _fieldIdentifier;

        [CascadingParameter] private EditContext EditContext { get; set; }
        [Parameter] public Expression<Func<TValue>> For { get; set; }
        [Parameter] public string Class { get; set; }

        protected IEnumerable<string> ValidationMessages => EditContext.GetValidationMessages(_fieldIdentifier);

        protected string ClassName =>
        new CssBuilder("validation-message")
            .AddClass(Class)
            .Build();

        protected override void OnInitialized()
        {
            _fieldIdentifier = FieldIdentifier.Create(For);
            EditContext.OnValidationStateChanged += HandleValidationStateChanged;
        }

        private void HandleValidationStateChanged(object o, ValidationStateChangedEventArgs args) => StateHasChanged();

        public void Dispose()
        {
            EditContext.OnValidationStateChanged -= HandleValidationStateChanged;
        }
    }
}
