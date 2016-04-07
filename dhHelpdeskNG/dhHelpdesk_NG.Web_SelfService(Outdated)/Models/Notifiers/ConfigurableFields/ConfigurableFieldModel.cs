namespace DH.Helpdesk.SelfService.Models.Notifiers.ConfigurableFields
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public abstract class ConfigurableFieldModel<TValue>
    {
        protected ConfigurableFieldModel()
        {
        }

        protected ConfigurableFieldModel(bool show)
        {
            this.Show = show;
        }

        protected ConfigurableFieldModel(bool show, string caption)
            : this(show)
        {
            this.Caption = caption;
        }

        public bool Show { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        public abstract TValue Value { get; set; }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }
    }
}