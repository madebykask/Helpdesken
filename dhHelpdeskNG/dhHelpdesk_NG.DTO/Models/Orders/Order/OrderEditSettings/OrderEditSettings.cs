namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderEditSettings
    {
         public OrderEditSettings(
                FieldEditSettings property, 
                TextFieldEditSettings orderRow1, 
                TextFieldEditSettings orderRow2, 
                TextFieldEditSettings orderRow3, 
                TextFieldEditSettings orderRow4, 
                TextFieldEditSettings orderRow5, 
                TextFieldEditSettings orderRow6, 
                TextFieldEditSettings orderRow7, 
                TextFieldEditSettings orderRow8, 
                TextFieldEditSettings configuration, 
                TextFieldEditSettings orderInfo, 
                TextFieldEditSettings orderInfo2)
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
        public FieldEditSettings Property { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderRow1 { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderRow2 { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderRow3 { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderRow4 { get; private set; }
 
        [NotNull]
        public TextFieldEditSettings OrderRow5 { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderRow6 { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderRow7 { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderRow8 { get; private set; }

        [NotNull]
        public TextFieldEditSettings Configuration { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderInfo { get; private set; }

        [NotNull]
        public TextFieldEditSettings OrderInfo2 { get; private set; }         
    }
}