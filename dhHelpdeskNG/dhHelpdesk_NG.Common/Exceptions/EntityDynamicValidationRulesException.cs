namespace DH.Helpdesk.Common.Exceptions
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

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
