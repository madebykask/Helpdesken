namespace DH.Helpdesk.Common.Exceptions
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ElementaryValidationRulesException : BusinessLogicException
    {
        public ElementaryValidationRulesException(string fieldName, string message)
            : base(message)
        {
            this.FieldName = fieldName;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }
    }
}
