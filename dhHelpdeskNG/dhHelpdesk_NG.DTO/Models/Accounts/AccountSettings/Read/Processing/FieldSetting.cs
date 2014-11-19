namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing
{
    public class FieldSetting
    {
        public FieldSetting(bool isShowInDetails, bool isRequired)
        {
            this.IsShowInDetails = isShowInDetails;
            this.IsRequired = isRequired;
        }

        public bool IsShowInDetails { get; private set; }

        public bool IsRequired { get; private set; }
    }
}
