namespace DH.Helpdesk.Mobile.Models.Notifiers.ConfigurableFields
{
    using DH.Helpdesk.Mobile.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    public sealed class DropDownFieldModel : ConfigurableFieldModel<DropDownContent>
    {
        public DropDownFieldModel()
        {
        }

        public DropDownFieldModel(bool show)
            : base(show)
        {
        }

        public DropDownFieldModel(bool show, string caption, DropDownContent value, bool required)
            : base(show, caption)
        {
            this.Value = value;
            this.Required = required;
        }

        public bool Required { get; set; }

        public override DropDownContent Value { get; set; }
    }
}