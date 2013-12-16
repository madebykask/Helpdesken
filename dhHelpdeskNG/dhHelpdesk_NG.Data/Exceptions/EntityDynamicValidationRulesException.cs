namespace dhHelpdesk_NG.Data.Exceptions
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Common.Tools;

    public sealed class EntityDynamicValidationRulesException : BusinessLogicException
    {
        public List<FieldValidationError> Errors { get; private set; }

        public EntityDynamicValidationRulesException(List<FieldValidationError> errors, string message)
            : base(message)
        {
            ArgumentsValidator.NotNull(errors, "errors");
            this.Errors = errors;
        }
    }
}
