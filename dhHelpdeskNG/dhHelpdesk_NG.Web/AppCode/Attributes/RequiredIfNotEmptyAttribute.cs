using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.AppCode.Attributes
{
	public class RequiredIfNotEmptyAttribute : ValidationAttribute, IClientValidatable
	{
		private string DependentProperty { get; set; }

		public RequiredIfNotEmptyAttribute(string dependentProperty)
		{
			DependentProperty = dependentProperty;
		}

		public override bool IsValid(object value)
		{
			return true;
		}

		public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
		{
			var rule = new ModelClientValidationRule
			{
				ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
				ValidationType = "requiredifnotempty",
			};

			rule.ValidationParameters.Add("dependentproperty", DependentProperty);

			yield return rule;
		}
	}
}