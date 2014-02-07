namespace DH.Helpdesk.Services.Validators
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.Exceptions;

    public abstract class DynamicRulesValidator
    {
        private const string ReadOnlyMessage = "The field is read-only.";

        private const string RequiredMessage = "The field is required.";

        private const string MinLengthMessage = "Insufficient field length.";

        protected void ValidateBooleanField(
            bool newValue,
            bool oldValue,
            string fieldName,
            FieldValidationSetting validationSetting,
            List<FieldValidationError> errors)
        {
            if (validationSetting.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName, errors);
            }
        }
        
        protected void ValidateIntegerField(
            int? value,
            string fieldName, 
            FieldValidationSetting validationSetting,
            List<FieldValidationError> errors)
        {
            if (validationSetting.ReadOnly)
            {
                ValidateReadOnly(value, fieldName, errors);
            }
            else if (validationSetting.Required)
            {
                ValidateRequired(value, fieldName, errors);
            }
        }

        protected void ValidateIntegerField(
            int? newValue,
            int? oldValue,
            string fieldName,
            FieldValidationSetting validationSetting,
            List<FieldValidationError> errors)
        {
            if (validationSetting.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName, errors);
            }
            else if (validationSetting.Required)
            {
                ValidateRequired(newValue, fieldName, errors);
            }
        }

        protected void ValidateStringField(
            string value,
            string fieldName,
            FieldValidationSetting validationSetting,
            List<FieldValidationError> errors)
        {
            if (validationSetting.ReadOnly)
            {
                ValidateReadOnly(value, fieldName, errors);
            }
            else
            {
                if (validationSetting.Required)
                {
                    ValidateRequired(value, fieldName, errors);
                }
            }
        }

        protected void ValidateStringField(
            string newValue,
            string oldValue,
            string fieldName,
            FieldValidationSetting validationSetting,
            List<FieldValidationError> errors)
        {
            if (validationSetting.ReadOnly)
            {
                ValidateReadOnly(newValue, oldValue, fieldName, errors);
            }
            else
            {
                if (validationSetting.Required)
                {
                    ValidateRequired(newValue, fieldName, errors);
                }
            }
        }

        private static void ValidateReadOnly(bool newValue, bool oldValue, string fieldName, List<FieldValidationError> errors)
        {
            if (newValue == oldValue)
            {
                return;
            }

            errors.Add(new FieldValidationError(fieldName, ReadOnlyMessage));
        }

        private static void ValidateReadOnly(int? value, string fieldName, List<FieldValidationError> errors)
        {
            if (value == null)
            {
                return;
            }

            errors.Add(new FieldValidationError(fieldName, ReadOnlyMessage));
        }

        private static void ValidateReadOnly(int? newValue, int? oldValue, string fieldName, List<FieldValidationError> errors)
        {
            if (newValue == oldValue)
            {
                return;
            }

            errors.Add(new FieldValidationError(fieldName, ReadOnlyMessage));
        }

        private static void ValidateReadOnly(string value, string fieldName, List<FieldValidationError> errors)
        {
            if (value == null)
            {
                return;
            }

            errors.Add(new FieldValidationError(fieldName, ReadOnlyMessage));
        }

        private static void ValidateReadOnly(string newValue, string oldValue, string fieldName, List<FieldValidationError> errors)
        {
            if (newValue == oldValue)
            {
                return;
            }

            errors.Add(new FieldValidationError(fieldName, ReadOnlyMessage));
        }

        private static void ValidateMinLength(string fieldValue, int minLength, string fieldName, List<FieldValidationError> errors)
        {
            if (fieldValue == null || fieldValue.Length >= minLength)
            {
                return;
            }

            errors.Add(new FieldValidationError(fieldName, MinLengthMessage));
        }

        private static void ValidateRequired(object fieldValue, string fieldName, List<FieldValidationError> errors)
        {
            if (fieldValue != null)
            {
                return;
            }

            errors.Add(new FieldValidationError(fieldName, RequiredMessage));
        }
    }
}