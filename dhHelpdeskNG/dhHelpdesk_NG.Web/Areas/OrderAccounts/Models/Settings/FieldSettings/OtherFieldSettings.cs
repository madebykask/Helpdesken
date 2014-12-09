namespace DH.Helpdesk.Web.Areas.OrderAccounts.Models.Settings.FieldSettings
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class OtherFieldSettings
    {
        public OtherFieldSettings(FieldSetting caseNumber, FieldSetting fileName, FieldSetting info)
        {
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
            this.Info = info;
        }

        [LocalizedDisplay("Ärendenummer")]
        public FieldSetting CaseNumber { get; private set; }

        [LocalizedDisplay("Filnamn")]
        public FieldSetting FileName { get; private set; }

        [LocalizedDisplay("Övrigt")]
        public FieldSetting Info { get; private set; }
    }
}