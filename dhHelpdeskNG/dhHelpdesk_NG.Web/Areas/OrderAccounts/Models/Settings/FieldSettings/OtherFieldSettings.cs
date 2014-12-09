namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class OtherFieldSettings
    {
        public OtherFieldSettings()
        {
        }

        public OtherFieldSettings(FieldSetting caseNumber, FieldSetting fileName, FieldSetting info)
        {
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
            this.Info = info;
        }

        [LocalizedDisplay("Ärendenummer")]
        public FieldSetting CaseNumber { get;  set; }

        [LocalizedDisplay("Filnamn")]
        public FieldSetting FileName { get;  set; }

        [LocalizedDisplay("Övrigt")]
        public FieldSetting Info { get;  set; }
    }
}