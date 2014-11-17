namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting
    {
        public FieldSetting(bool showInDetails, bool showInList, string caption, string help, bool isRequired)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Caption = caption;
            this.Help = help;
            this.IsRequired = isRequired;
        }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public string Help { get; private set; }

        public bool IsRequired { get; private set; }
    }
}
