namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes.Rules;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LocalizedRequiredFromAttribute : ConditionalValidationAttribute, IClientValidatable
    {
        #region Fields

        [NotNullAndEmpty]
        private readonly string dependencyPropertyName;

        #endregion

        #region Constructors and Destructors

        public LocalizedRequiredFromAttribute(string dependencyProperty)
        {
            this.dependencyPropertyName = dependencyProperty;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var errorMessage = Translation.GetCoreTextTranslation("fältet är obligatoriskt");
            return new List<ModelClientValidationRule> { new ModelClientValidationRequiredFromRule(errorMessage) };
        }

        #endregion

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isRequired = ReflectionHelper.GetInstancePropertyValue<bool>(
                validationContext.ObjectInstance,
                this.dependencyPropertyName);

            if (isRequired && value == null)
            {
                return new ValidationResult("The field is required.");
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}