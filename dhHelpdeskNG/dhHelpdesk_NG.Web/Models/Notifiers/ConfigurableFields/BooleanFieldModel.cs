namespace DH.Helpdesk.Web.Models.Notifiers.ConfigurableFields
{
    public sealed class BooleanFieldModel : ConfigurableFieldModel<bool>
    {
        public BooleanFieldModel()
        {
        }

        public BooleanFieldModel(bool show)
            : base(show)
        {
        }

        public BooleanFieldModel(bool show, string caption, bool value)
            : base(show, caption)
        {
            this.Value = value;
        }

        public override bool Value { get; set; }
    }
}