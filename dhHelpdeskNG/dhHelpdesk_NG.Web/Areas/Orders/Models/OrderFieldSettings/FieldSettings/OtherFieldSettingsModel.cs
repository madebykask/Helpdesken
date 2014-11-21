namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class OtherFieldSettingsModel
    {
        public OtherFieldSettingsModel()
        {            
        }

        public OtherFieldSettingsModel(
                TextFieldSettingsModel fileName, 
                TextFieldSettingsModel caseNumber, 
                TextFieldSettingsModel info, 
                TextFieldSettingsModel status)
        {
            this.Status = status;
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        [NotNull]
        [LocalizedDisplay("Filnamn")]
        public TextFieldSettingsModel FileName { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Ärendenummer")]
        public TextFieldSettingsModel CaseNumber { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Övrigt")]
        public TextFieldSettingsModel Info { get; set; }
         
        [NotNull]
        [LocalizedDisplay("Status")]
        public TextFieldSettingsModel Status { get; set; }   
    }
}