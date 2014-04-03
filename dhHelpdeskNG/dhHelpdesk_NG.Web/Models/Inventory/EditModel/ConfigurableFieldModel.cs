namespace DH.Helpdesk.Web.Models.Inventory.EditModel
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ConfigurableFieldModel<TValue>
    {
        public ConfigurableFieldModel()
        {
        }

        public ConfigurableFieldModel(bool show)
        {
            this.Show = show;
        }

        public ConfigurableFieldModel(bool show, string caption, TValue value, bool isRequired, bool isReadOnly)
        {
            this.Show = show;
            this.Caption = caption;
            this.Value = value;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
        }

        public bool Show { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public TValue Value { get; set; }

        public bool IsRequired { get; set; }

        public bool IsReadOnly { get; set; }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }
    }
}