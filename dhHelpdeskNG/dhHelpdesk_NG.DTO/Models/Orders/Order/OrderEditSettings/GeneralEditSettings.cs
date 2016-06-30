namespace DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralEditSettings
    {
         public GeneralEditSettings(
                FieldEditSettings orderNumber, 
                FieldEditSettings customer, 
                TextFieldEditSettings administrator, 
                TextFieldEditSettings domain,
                TextFieldEditSettings orderDate,
                TextFieldEditSettings status)
        {
            this.OrderDate = orderDate;
            this.Domain = domain;
            this.Administrator = administrator;
            this.Customer = customer;
            this.OrderNumber = orderNumber;
            this.Status = status;
        }

        [NotNull]
        public FieldEditSettings OrderNumber { get; private set;}
    
        [NotNull]
        public FieldEditSettings Customer { get; private set;}
    
        [NotNull]
        public TextFieldEditSettings Administrator { get; private set; }
    
        [NotNull]
        public TextFieldEditSettings Domain { get; private set; }
    
        [NotNull]
        public TextFieldEditSettings OrderDate { get; private set; }

        [NotNull]
        public TextFieldEditSettings Status { get; private set; } 
    }
}