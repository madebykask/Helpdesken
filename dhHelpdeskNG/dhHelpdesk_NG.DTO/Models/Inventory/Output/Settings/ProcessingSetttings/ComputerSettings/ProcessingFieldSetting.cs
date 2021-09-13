namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.ComputerSettings
{
    public class ProcessingFieldSetting
    {
        public ProcessingFieldSetting(bool isShow, bool isRequired, bool isReadOnly, bool isCopy)
        {
            this.IsShow = isShow;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
            IsCopy = isCopy;
        }

        public bool IsShow { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsReadOnly { get; private set; }
        public bool IsCopy { get; private set; }
    }
}
