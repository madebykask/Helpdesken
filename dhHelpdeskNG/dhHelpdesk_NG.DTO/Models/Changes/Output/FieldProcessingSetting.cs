namespace DH.Helpdesk.BusinessData.Models.Changes.Output
{
    public sealed class FieldProcessingSetting
    {
        public FieldProcessingSetting(bool show, bool required)
        {
            this.Show = show;
            this.Required = required;
        }

        public bool Show { get; private set; }

        public bool Required { get; private set; }
    }
}
