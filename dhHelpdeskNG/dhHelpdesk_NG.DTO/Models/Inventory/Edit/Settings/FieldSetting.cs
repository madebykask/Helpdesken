namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting
    {
        public FieldSetting(
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Caption = caption;
            this.IsRequired = isRequired;
        }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public bool IsRequired { get; private set; }
    }
}
