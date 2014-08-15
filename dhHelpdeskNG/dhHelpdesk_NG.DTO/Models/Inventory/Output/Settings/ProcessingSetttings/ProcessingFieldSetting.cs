namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings
{
    public class ProcessingFieldSetting
    {
        public ProcessingFieldSetting(bool isShow, bool isRequired)
        {
            this.IsShow = isShow;
            this.IsRequired = isRequired;
        }

        public bool IsShow { get; private set; }

        public bool IsRequired { get; private set; }
    }
}
