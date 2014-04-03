namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common.Concrete
{
    using System;

    using DH.Helpdesk.Services.Exceptions;

    public sealed class ElementaryRulesValidator : IElementaryRulesValidator
    {
        private const string ReadOnlyMessage = "The field is read-only.";

        private const string RequiredMessage = "The field is required.";

        public void ValidateDateTimeField(
            DateTime? newValue,
            DateTime? oldValue,
            string fieldName,
            ElementaryValidationRule rule)
        {
            if (rule.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName);
            }
            else if (rule.Required)
            {
                ValidateRequired(newValue, fieldName);
            }
        }

        public void ValidateBooleanField(bool newValue, bool oldValue, string fieldName, ElementaryValidationRule rule)
        {
            if (rule.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName);
            }
        }

        public void ValidateIntegerField(int? value, string fieldName, ElementaryValidationRule rule)
        {
            if (rule.ReadOnly)
            {
                ValidateReadOnly(value, fieldName);
            }
            else if (rule.Required)
            {
                ValidateRequired(value, fieldName);
            }
        }

        public void ValidateRealField(
            double? newValue,
            double? oldValue,
            string fieldName,
            ElementaryValidationRule rule)
        {
            if (rule.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName);
            }

            if (rule.Required)
            {
                ValidateRequired(newValue, fieldName);
            }
        }

        public void ValidateIntegerField(int? newValue, int? oldValue, string fieldName, ElementaryValidationRule rule)
        {
            if (rule.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName);
            }
            else if (rule.Required)
            {
                ValidateRequired(newValue, fieldName);
            }
        }

        public void ValidateStringField(string value, string fieldName, ElementaryValidationRule rule)
        {
            if (rule.ReadOnly)
            {
                ValidateReadOnly(value, fieldName);
            }
            else if (rule.Required)
            {
                ValidateRequired(value, fieldName);
            }
        }

        public void ValidateStringField(
            string newValue,
            string oldValue,
            string fieldName,
            ElementaryValidationRule rule)
        {
            if (rule.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName);
            }
            else if (rule.Required)
            {
                ValidateRequired(newValue, fieldName);
            }
        }

        private static void ValidateReadOnly(int? value, string fieldName)
        {
            if (value == null)
            {
                return;
            }

            throw new ElementaryValidationRulesException(fieldName, ReadOnlyMessage);
        }

        private static void ValidateReadOnly(string value, string fieldName)
        {
            if (value == null)
            {
                return;
            }

            throw new ElementaryValidationRulesException(fieldName, ReadOnlyMessage);
        }

        private static void ValidateReadOnly(object newValue, object oldValue, string fieldName)
        {
            if (object.Equals(newValue, oldValue))
            {
                return;
            }

            throw new ElementaryValidationRulesException(fieldName, ReadOnlyMessage);
        }

        private static void ValidateRequired(object fieldValue, string fieldName)
        {
            if (fieldValue != null)
            {
                return;
            }

            throw new ElementaryValidationRulesException(fieldName, RequiredMessage);
        }
    }
}