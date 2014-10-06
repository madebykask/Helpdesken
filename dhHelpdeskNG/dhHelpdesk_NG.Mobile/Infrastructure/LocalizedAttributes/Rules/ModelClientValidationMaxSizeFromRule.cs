namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes.Rules
{
    using System.Web.Mvc;

    public sealed class ModelClientValidationMaxSizeFromRule : ModelClientValidationRule
    {
        #region Constructors and Destructors

        public ModelClientValidationMaxSizeFromRule(string errorMessage)
        {
            this.ValidationType = "maxsizefrom";
            this.ErrorMessage = errorMessage;
        }

        #endregion
    }
}   