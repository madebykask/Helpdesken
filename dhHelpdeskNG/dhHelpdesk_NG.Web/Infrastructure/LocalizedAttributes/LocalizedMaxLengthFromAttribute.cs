namespace DH.Helpdesk.Web.Infrastructure.LocalizedAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes.Rules;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LocalizedMaxLengthFromAttribute : ConditionalValidationAttribute, IClientValidatable
    {
        [NotNullAndEmpty]
        private readonly string dependencyPropertyName;

        public LocalizedMaxLengthFromAttribute(string dependencyProperty)
        {
            this.dependencyPropertyName = dependencyProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var maxLength = ReflectionHelper.GetInstancePropertyValue<int>(
                validationContext.ObjectInstance,
                this.dependencyPropertyName);

            return ((string)value).Length > maxLength
                ? new ValidationResult("The maximum length is " + maxLength + " character(s).")
                : ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var maxLength = this.GetInstancePropertyValue<int>(context, this.dependencyPropertyName);

            var errorMessage = Translation.GetCoreTextTranslation("maximum length is ")
                               + maxLength + Translation.GetCoreTextTranslation(" character(s)");

            return new List<ModelClientValidationRule>
                   {
                       new ModelClientValidationMaxLengthFromRule(
                           maxLength,
                           errorMessage)
                   };
        }
    }
}