using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Web.Infrastructure;

namespace DH.Helpdesk.Web.AppCode.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RequiredIfNotEmptyAttribute : ValidationAttribute, IClientValidatable
	{
		private string DependentProperty { get; set; }
		private const string DefaultErrorMessage = "Fältet {0} krävs";

		public RequiredIfNotEmptyAttribute(string dependentProperty)
		{
			DependentProperty = dependentProperty;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var containerType = validationContext.ObjectInstance.GetType();
			var dependField = containerType.GetProperty(DependentProperty);

			if (value != null)
			{
				var dependentValue = dependField?.GetValue(validationContext.ObjectInstance, null);
				if (dependentValue != null)
				{
					return ValidationResult.Success;
				}

				var fieldDisplayName = string.Empty;
				if (dependField != null)
				{
					var nameAttribute = dependField.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault() as DisplayNameAttribute;
					fieldDisplayName = nameAttribute != null ? Translation.GetCoreTextTranslation(nameAttribute.DisplayName) : string.Empty;
				}
				
				var errorMessage = GetErrorMessage(fieldDisplayName);
				return new ValidationResult(errorMessage);
			}
			return ValidationResult.Success;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var fieldName = string.IsNullOrEmpty(metadata.DisplayName) ? string.Empty : Translation.GetCoreTextTranslation(metadata.GetDisplayName());
			var errorMessage = GetErrorMessage(fieldName);
			var rule = new ModelClientValidationRule
			{
				ErrorMessage = errorMessage,
				ValidationType = "requiredifnotempty",
			};

			rule.ValidationParameters.Add("dependentproperty", DependentProperty);

			yield return rule;
		}

		private string GetErrorMessage(string fieldDisplayName)
		{
			return string.IsNullOrEmpty(ErrorMessage)
				? string.Format(Translation.GetCoreTextTranslation(DefaultErrorMessage), fieldDisplayName)
				: string.Format(Translation.GetCoreTextTranslation(ErrorMessage), fieldDisplayName);
		}
	}
}