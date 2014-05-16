namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes.Rules
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