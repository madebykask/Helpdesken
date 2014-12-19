namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelValidators.Common
{
    using System;

    public interface IElementaryRulesValidator
    {
        void ValidateDateTimeField(
            DateTime? newValue,
            DateTime? oldValue,
            string fieldName,
            ElementaryValidationRule rule);

        void ValidateDateTimeField(DateTime? newValue, string fieldName, ElementaryValidationRule rule);

        void ValidateBooleanField(bool newValue, bool oldValue, string fieldName, ElementaryValidationRule rule);

        void ValidateBooleanField(bool newValue, string fieldName, ElementaryValidationRule rule);

        void ValidateIntegerField(int? value, string fieldName, ElementaryValidationRule rule);

        void ValidateIntegerField(int value, string fieldName, ElementaryValidationRule rule);

        void ValidateRealField(double? newValue, double? oldValue, string fieldName, ElementaryValidationRule rule);

        void ValidateDecimalField(decimal? newValue, decimal? oldValue, string fieldName, ElementaryValidationRule rule);

        void ValidateIntegerField(int? newValue, int? oldValue, string fieldName, ElementaryValidationRule rule);

        void ValidateStringField(string value, string fieldName, ElementaryValidationRule rule);

        void ValidateStringField(string newValue, string oldValue, string fieldName, ElementaryValidationRule rule);

        void ValidateForNew(object value, string fieldName, ElementaryValidationRule rule);

        void ValidateRealField(decimal value, string fieldName, ElementaryValidationRule rule);
    }
}