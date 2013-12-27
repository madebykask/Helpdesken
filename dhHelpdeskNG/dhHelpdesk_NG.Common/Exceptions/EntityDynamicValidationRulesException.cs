namespace dhHelpdesk_NG.Common.Exceptions
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.ValidationAttributes;

    public sealed class EntityDynamicValidationRulesException : BusinessLogicException
    {
        public EntityDynamicValidationRulesException(List<FieldValidationError> errors, string message)
            : base(message)
        {
            this.Errors = errors;
        }

        [NotNullAndEmptyCollection]
        public List<FieldValidationError> Errors { get; private set; }
    }
}
