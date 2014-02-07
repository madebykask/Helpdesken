namespace DH.Helpdesk.Web.Infrastructure.ModelMetaData
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "{0} must be at least {1} characters long.";
        private readonly int _minCharacters = 8;

        public ValidatePasswordLengthAttribute(int minCharacters)
            : base(_defaultErrorMessage)
        {
            this._minCharacters = minCharacters;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, this.ErrorMessageString,
                name, this._minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= this._minCharacters);
        }
    }
}