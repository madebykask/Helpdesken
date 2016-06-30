namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OtherFieldSettings
    {
        public OtherFieldSettings(
                TextFieldSettings fileName, 
                TextFieldSettings caseNumber, 
                TextFieldSettings info)
        {
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        [NotNull]
        public TextFieldSettings FileName { get; private set; }
         
        [NotNull]
        public TextFieldSettings CaseNumber { get; private set; }
         
        [NotNull]
        public TextFieldSettings Info { get; private set; }
            
    }
}