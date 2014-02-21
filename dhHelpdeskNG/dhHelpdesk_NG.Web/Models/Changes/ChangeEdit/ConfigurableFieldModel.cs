namespace DH.Helpdesk.Web.Models.Changes.ChangeEdit
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

        public ConfigurableFieldModel(bool show, string caption, TValue value, bool required)
        {
            this.Show = show;
            this.Caption = caption;
            this.Value = value;
            this.Required = required;
        }

        public bool Show { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public TValue Value { get; set; }

        public bool Required { get; set; }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }
    }
}