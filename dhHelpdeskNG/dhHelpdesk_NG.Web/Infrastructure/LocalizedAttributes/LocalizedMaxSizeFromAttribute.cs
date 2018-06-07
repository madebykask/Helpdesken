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
    public sealed class LocalizedMaxSizeFromAttribute : ConditionalValidationAttribute, IClientValidatable
    {
        #region Fields

        [NotNullAndEmpty]
        private readonly string dependencyPropertyName;

        [NotNullAndEmpty]
        private readonly string dependencyMaxSizePropertyName;

        #endregion

        #region Constructors and Destructors

        public LocalizedMaxSizeFromAttribute(string dependencyProperty, string dependencyMaxSizePropertyName)
        {
            this.dependencyPropertyName = dependencyProperty;
            this.dependencyMaxSizePropertyName = dependencyMaxSizePropertyName;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var errorMessage = Translation.GetCoreTextTranslation("Max size was exceeded");
            return new List<ModelClientValidationRule> { new ModelClientValidationMaxSizeFromRule(errorMessage) };
        }

        #endregion

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isUseMaxSize = ReflectionHelper.GetInstancePropertyValue<bool>(
                validationContext.ObjectInstance,
                this.dependencyPropertyName);

            if (!isUseMaxSize)
            {
                return ValidationResult.Success;
            }

            var maxSize = ReflectionHelper.GetInstancePropertyValue<int>(
                validationContext.ObjectInstance,
                this.dependencyMaxSizePropertyName);


            if (value != null && ((string)value).Length > maxSize)
            {
                return new ValidationResult("Max size was exceeded");
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}