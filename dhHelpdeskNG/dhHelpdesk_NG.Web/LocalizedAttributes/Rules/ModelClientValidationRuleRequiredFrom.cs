namespace DH.Helpdesk.Web.LocalizedAttributes.Rules
{
    using System.Web.Mvc;

    public sealed class ModelClientValidationRuleRequiredFrom : ModelClientValidationRule
    {
        public ModelClientValidationRuleRequiredFrom(bool isRequired, string errorMessage)
        {
            this.ValidationType = "requiredfrom";
            this.ValidationParameters["isrequired"] = isRequired;
            this.ErrorMessage = errorMessage;
        }
    }
}