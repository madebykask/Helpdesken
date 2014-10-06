namespace DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes.Rules
{
    using System.Web.Mvc;

    public sealed class ModelClientValidationMaxLengthFromRule : ModelClientValidationRule
    {
        public ModelClientValidationMaxLengthFromRule(int maxLength, string errorMessage)
        {
            this.ValidationType = "maxlengthfrom";
            this.ValidationParameters["maxlength"] = maxLength;
            this.ErrorMessage = errorMessage;
        }
    }
}