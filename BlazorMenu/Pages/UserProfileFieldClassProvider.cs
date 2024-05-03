using Microsoft.AspNetCore.Components.Forms;

namespace BlazorMenu.Pages
{
    internal sealed class UserProfileFieldClassProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            return isValid ? "is-valid" : "is-invalid";
        }
    }
}
