namespace DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes.Rules
{
    using System.Web.Mvc;

    public sealed class ModelClientValidationRequiredFromRule : ModelClientValidationRule
    {
        #region Constructors and Destructors

        public ModelClientValidationRequiredFromRule(string errorMessage)
        {
            this.ValidationType = "requiredfrom";
            this.ErrorMessage = errorMessage;
        }

        #endregion
    }
}