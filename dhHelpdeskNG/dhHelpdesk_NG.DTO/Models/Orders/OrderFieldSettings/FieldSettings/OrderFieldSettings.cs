namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderFieldSettings
    {
        [NotNull]
        public TextFieldSettings Property { get; private set;}

        [NotNull]
        public FieldSettings OrderRow1 { get; private set; }

        [NotNull]
        public FieldSettings OrderRow2 { get; private set; }

        [NotNull]
        public FieldSettings OrderRow3 { get; private set; }

        [NotNull]
        public FieldSettings OrderRow4 { get; private set;}
 
        [NotNull]
        public FieldSettings OrderRow5 { get; private set; }

        [NotNull]
        public FieldSettings OrderRow6 { get; private set; }

        [NotNull]
        public FieldSettings OrderRow7 { get; private set; }

        [NotNull]
        public FieldSettings OrderRow8 { get; private set; }

        [NotNull]
        public FieldSettings Configuration { get; private set; }

        [NotNull]
        public FieldSettings OrderInfo { get; private set; }

        [NotNull]
        public FieldSettings OrderInfo2 { get; private set; }         
    }
}