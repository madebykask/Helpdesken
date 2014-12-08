namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings
{
    public class FieldSettingMultipleChoices : FieldSetting
    {
        public FieldSettingMultipleChoices(
            bool isShowInDetails,
            bool isShowInList,
            bool isShowExternal,
            string caption,
            string help,
            bool isRequired,
            bool isMultiple)
            : base(isShowInDetails, isShowInList, isShowExternal, caption, help, isRequired)
        {
            this.IsMultiple = isMultiple;
        }

        public bool IsMultiple { get; private set; }
    }
}