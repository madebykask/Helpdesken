using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

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
                TextFieldSettingsModel info)
        {
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OtherFileName)]
        public TextFieldSettingsModel FileName { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.OtherCaseNumber)]
        public TextFieldSettingsModel CaseNumber { get; set; }
         
        [NotNull]
        [LocalizedDisplay(OrderLabels.OtherInfo)]
        public TextFieldSettingsModel Info { get; set; }
           
    }
}