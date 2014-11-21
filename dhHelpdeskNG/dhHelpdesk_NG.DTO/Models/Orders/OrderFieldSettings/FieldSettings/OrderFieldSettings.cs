namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderFieldSettings
    {
        public OrderFieldSettings(
                FieldSettings property, 
                TextFieldSettings orderRow1, 
                TextFieldSettings orderRow2, 
                TextFieldSettings orderRow3, 
                TextFieldSettings orderRow4, 
                TextFieldSettings orderRow5, 
                TextFieldSettings orderRow6, 
                TextFieldSettings orderRow7, 
                TextFieldSettings orderRow8, 
                TextFieldSettings configuration, 
                TextFieldSettings orderInfo, 
                TextFieldSettings orderInfo2)
        {
            this.OrderInfo2 = orderInfo2;
            this.OrderInfo = orderInfo;
            this.Configuration = configuration;
            this.OrderRow8 = orderRow8;
            this.OrderRow7 = orderRow7;
            this.OrderRow6 = orderRow6;
            this.OrderRow5 = orderRow5;
            this.OrderRow4 = orderRow4;
            this.OrderRow3 = orderRow3;
            this.OrderRow2 = orderRow2;
            this.OrderRow1 = orderRow1;
            this.Property = property;
        }

        [NotNull]
        public FieldSettings Property { get; private set; }

        [NotNull]
        public TextFieldSettings OrderRow1 { get; private set; }

        [NotNull]
        public TextFieldSettings OrderRow2 { get; private set; }

        [NotNull]
        public TextFieldSettings OrderRow3 { get; private set; }

        [NotNull]
        public TextFieldSettings OrderRow4 { get; private set; }
 
        [NotNull]
        public TextFieldSettings OrderRow5 { get; private set; }

        [NotNull]
        public TextFieldSettings OrderRow6 { get; private set; }

        [NotNull]
        public TextFieldSettings OrderRow7 { get; private set; }

        [NotNull]
        public TextFieldSettings OrderRow8 { get; private set; }

        [NotNull]
        public TextFieldSettings Configuration { get; private set; }

        [NotNull]
        public TextFieldSettings OrderInfo { get; private set; }

        [NotNull]
        public TextFieldSettings OrderInfo2 { get; private set; }         
    }
}