﻿namespace DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.SelfService.Infrastructure;
    using DH.Helpdesk.SelfService.Infrastructure.LocalizedAttributes.Rules;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class LocalizedRequiredFromAttribute : ConditionalValidationAttribute, IClientValidatable
    {
        [NotNullAndEmpty]
        private readonly string dependencyPropertyName;

        public LocalizedRequiredFromAttribute(string dependencyProperty)
        {
            this.dependencyPropertyName = dependencyProperty;
        }

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

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(
            ModelMetadata metadata,
            ControllerContext context)
        {
            var isRequired = this.GetInstancePropertyValue<bool>(context, this.dependencyPropertyName);
            var errorMessage = Translation.Get("required", Enums.TranslationSource.TextTranslation);

            return new List<ModelClientValidationRule>
                   {
                       new ModelClientValidationRequiredFromRule(
                           isRequired,
                           errorMessage)
                   };
        }
    }
}