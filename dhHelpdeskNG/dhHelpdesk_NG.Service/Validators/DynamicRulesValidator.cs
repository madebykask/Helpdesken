namespace dhHelpdesk_NG.Service.Validators
{
    using dhHelpdesk_NG.Data.Exceptions;

    public abstract class DynamicRulesValidator
    {
        protected void IsNotNull(object field, string fieldName)
        {
            if (field == null)
            {
                throw new EntityDynamicValidationRulesException("1");
            }
        }

        protected void IsNotNullAndEmpty(string field, string fieldName)
        {
            if (string.IsNullOrEmpty(field))
            {
                throw new EntityDynamicValidationRulesException("1");
            }
        }

        protected void HasMinLength(string field, int minLength, string fieldName)
        {
            if (field.Length < minLength)
            {
                throw new EntityDynamicValidationRulesException("1");
            }
        }
    }
}