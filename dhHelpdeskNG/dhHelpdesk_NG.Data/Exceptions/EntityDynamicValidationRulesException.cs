namespace dhHelpdesk_NG.Data.Exceptions
{
    public sealed class EntityDynamicValidationRulesException : BusinessLogicException
    {
        public EntityDynamicValidationRulesException(string message)
            : base(message)
        {
        }
    }
}
