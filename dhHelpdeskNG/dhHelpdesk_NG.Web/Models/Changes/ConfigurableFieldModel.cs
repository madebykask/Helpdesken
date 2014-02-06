namespace dhHelpdesk_NG.Web.Models.Changes
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class ConfigurableFieldModel<TValue>
    {
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
    }
}