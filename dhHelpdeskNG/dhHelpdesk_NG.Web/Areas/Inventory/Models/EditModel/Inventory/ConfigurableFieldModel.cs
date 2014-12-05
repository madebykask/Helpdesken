namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ConfigurableFieldModel<TValue>
    {
        public ConfigurableFieldModel()
        {
        }

        public ConfigurableFieldModel(string caption, TValue value)
        {
            this.Show = true;
            this.Caption = caption;
            this.Value = value;
        }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public bool Show { get; set; }

        public TValue Value { get; set; }

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