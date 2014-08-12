namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    public class ProcessingFieldSetting
    {
        public ProcessingFieldSetting(bool isShow, bool isRequired, bool isReadOnly)
        {
            this.IsShow = isShow;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
        }

        public bool IsShow { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsReadOnly { get; private set; }
    }
}
