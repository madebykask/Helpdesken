namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Write
{
    public class FieldSettingMultipleChoices : FieldSetting
    {
        public FieldSettingMultipleChoices(
            bool showInDetails,
            bool showInList,
            string caption,
            string help,
            bool isRequired,
            bool isMultiple)
            : base(showInDetails, showInList, caption, help, isRequired)
        {
            this.IsMultiple = isMultiple;
        }

        public bool IsMultiple { get; private set; }
    }
}