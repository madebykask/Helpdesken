namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Order.FieldModels
{
    public sealed class ConfigurableFieldModelMultipleChoices<TValue> : ConfigurableFieldModel<TValue>
    {
        public ConfigurableFieldModelMultipleChoices()
        {
        }

        public ConfigurableFieldModelMultipleChoices(string caption, TValue value, bool required, bool isMultiple)
            : base(caption, value, required)
        {
            this.IsMultiple = isMultiple;
        }

        public bool IsMultiple { get; set; }
    }
}