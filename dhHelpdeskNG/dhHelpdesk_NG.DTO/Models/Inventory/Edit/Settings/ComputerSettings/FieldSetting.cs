namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting
    {
        public FieldSetting(
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired,
            bool isReadOnly)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Caption = caption;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
        }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsReadOnly { get; private set; }
    }
}
