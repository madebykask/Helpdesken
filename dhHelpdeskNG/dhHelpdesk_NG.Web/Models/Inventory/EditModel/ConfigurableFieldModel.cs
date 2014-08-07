namespace DH.Helpdesk.Web.Models.Inventory.EditModel
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ConfigurableFieldModel<TValue> : IConfigurableFieldModel
    {
        public ConfigurableFieldModel()
        {
        }

        public ConfigurableFieldModel(string caption, TValue value, bool isRequired)
        {
            this.Show = true;
            this.Caption = caption;
            this.Value = value;
            this.IsRequired = isRequired;
        }

        public bool Show { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        //[LocalizedRequiredFrom("Show")]
        public TValue Value { get; set; }

        public bool IsRequired { get; set; }

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