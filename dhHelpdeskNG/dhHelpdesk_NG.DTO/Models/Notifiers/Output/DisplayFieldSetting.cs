namespace DH.Helpdesk.BusinessData.Models.Notifiers.Output
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class DisplayFieldSetting
    {
        public DisplayFieldSetting(bool show, string caption, bool required)
        {
            this.Show = show;
            this.Caption = caption;
            this.Required = required;
        }

        [NotNull]
        public string Caption { get; private set; }

        public bool Show { get; private set; }

        public bool Required { get; private set; }
    }
}
