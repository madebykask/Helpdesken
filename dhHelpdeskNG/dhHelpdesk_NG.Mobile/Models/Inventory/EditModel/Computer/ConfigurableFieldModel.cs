namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Mobile.Infrastructure.LocalizedAttributes;

    public class ConfigurableFieldModel<TValue> : IConfigurableFieldModel
    {
        public ConfigurableFieldModel()
        {
        }

        public ConfigurableFieldModel(string caption, TValue value, bool isRequired, bool isReadOnly)
        {
            this.Show = true;
            this.Caption = caption;
            this.Value = value;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
        }

        public bool Show { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        [LocalizedRequiredFrom("IsRequired")]
        public TValue Value { get; set; }

        public bool IsRequired { get; set; }

        public bool IsReadOnly { get; set; }

        public static ConfigurableFieldModel<TValue> CreateUnshowable()
        {
            return new ConfigurableFieldModel<TValue> { Show = false };
        }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }
    }
}