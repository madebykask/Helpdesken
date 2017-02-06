using DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings;

namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OtherEditSettings : HeaderSettings
    {
        public OtherEditSettings(
                TextFieldEditSettings fileName, 
                TextFieldEditSettings caseNumber, 
                TextFieldEditSettings info)
        {
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        [NotNull]
        public TextFieldEditSettings FileName { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings CaseNumber { get; private set; }
         
        [NotNull]
        public TextFieldEditSettings Info { get; private set; }
             
    }
}