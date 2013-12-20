namespace dhHelpdesk_NG.Common.Exceptions
{
    using dhHelpdesk_NG.Common.Tools;

    public sealed class FieldValidationError
    {
        public FieldValidationError(string fieldName, string message)
        {
            ArgumentsValidator.NotNullAndEmpty(fieldName, "fieldName");
            ArgumentsValidator.NotNullAndEmpty(message, "message");

            this.FieldName = fieldName;
            this.Message = message;
        }

        public string FieldName { get; private set; }

        public string Message { get; private set; }
    }
}
