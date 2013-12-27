namespace dhHelpdesk_NG.Common.Exceptions
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class FieldValidationError
    {
        public FieldValidationError(string fieldName, string message)
        {
            this.FieldName = fieldName;
            this.Message = message;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        [NotNullAndEmpty]
        public string Message { get; private set; }
    }
}
