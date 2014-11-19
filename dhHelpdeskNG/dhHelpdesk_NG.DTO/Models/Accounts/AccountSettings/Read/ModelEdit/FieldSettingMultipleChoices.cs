namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.ModelEdit
{
    public class FieldSettingMultipleChoices : FieldSetting
    {
        public FieldSettingMultipleChoices(
            bool isShowInDetails,
            string caption,
            string help,
            bool isRequired,
            bool isMultiple)
            : base(isShowInDetails, caption, help, isRequired)
        {
            this.IsMultiple = isMultiple;
        }

        public bool IsMultiple { get; private set; }
    }
}