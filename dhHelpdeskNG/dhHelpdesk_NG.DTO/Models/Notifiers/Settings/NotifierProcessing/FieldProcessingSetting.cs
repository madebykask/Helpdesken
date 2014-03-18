namespace DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierProcessing
{
    public class FieldProcessingSetting
    {
        public FieldProcessingSetting(bool show, bool required)
        {
            this.Show = show;
            this.Required = required;
        }

        public bool Required { get; private set; }

        public bool Show { get; private set; }
    }
}
