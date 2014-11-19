namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Overview
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSetting
    {
        public FieldSetting(bool isShowInList, string caption)
        {
            this.IsShowInList = isShowInList;
            this.Caption = caption;
        }

        public bool IsShowInList { get; private set; }

        [NotNull]
        public string Caption { get; private set; }
    }
}
