namespace DH.Helpdesk.BusinessData.Models.Accounts.AccountSettings.Read.Processing
{
    public class OtherFieldSettings
    {
        public OtherFieldSettings(FieldSetting caseNumber, FieldSetting fileName, FieldSetting info)
        {
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
            this.Info = info;
        }

        public FieldSetting CaseNumber { get; private set; }

        public FieldSetting FileName { get; private set; }

        public FieldSetting Info { get; private set; }
    }
}