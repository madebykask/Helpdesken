namespace DH.Helpdesk.Services.Exceptions
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ElementaryValidationRulesException : BusinessLogicException
    {
        public ElementaryValidationRulesException(string fieldName, string message)
            : base(string.Format(@"FieldName: ""{0}""; Message: ""{1}"".", fieldName, message))
        {
            this.FieldName = fieldName;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }       
    }
}
