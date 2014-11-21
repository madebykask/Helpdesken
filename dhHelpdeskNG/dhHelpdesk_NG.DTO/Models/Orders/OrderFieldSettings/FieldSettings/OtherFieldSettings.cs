namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OtherFieldSettings
    {
        public OtherFieldSettings(
                FieldSettings fileName, 
                FieldSettings caseNumber, 
                FieldSettings info, 
                FieldSettings status)
        {
            this.Status = status;
            this.Info = info;
            this.CaseNumber = caseNumber;
            this.FileName = fileName;
        }

        [NotNull]
        public FieldSettings FileName { get; private set; }
         
        [NotNull]
        public FieldSettings CaseNumber { get; private set; }
         
        [NotNull]
        public FieldSettings Info { get; private set; }
         
        [NotNull]
        public FieldSettings Status { get; private set; }         
    }
}