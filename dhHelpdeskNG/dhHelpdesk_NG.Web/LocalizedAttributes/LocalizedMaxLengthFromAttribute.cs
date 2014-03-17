namespace DH.Helpdesk.Web.LocalizedAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.LocalizedAttributes.Rules;

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
            return ValidationResult.Success;
            if (value == null)
            {
                return ValidationResult.Success;
            }

            var maxLength = ReflectionHelper.GetPropertyValue<int>(
              validationContext.ObjectInstance,
              this.dependencyPropertyName);

            if (((string)value).Length > maxLength)
            {
                return new ValidationResult("Max length increased.");
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var maxLength = this.GetInstancePropertyValue<int>(context, this.dependencyPropertyName);

            var errorMessage = Translation.Get("max length is: ", Enums.TranslationSource.TextTranslation) + maxLength
                               + Translation.Get(" character's", Enums.TranslationSource.TextTranslation);

            return new List<ModelClientValidationRule>
                   {
                       new ModelClientValidationMaxLengthFromRule(
                           maxLength,
                           errorMessage)
                   };
        }
    }
}