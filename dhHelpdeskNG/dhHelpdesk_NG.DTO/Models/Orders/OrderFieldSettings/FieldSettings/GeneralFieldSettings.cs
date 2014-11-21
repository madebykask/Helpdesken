namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralFieldSettings
    {
        public GeneralFieldSettings(
                TextFieldSettings orderNumber, 
                TextFieldSettings customer, 
                FieldSettings administrator, 
                FieldSettings domain, 
                FieldSettings orderDate)
        {
            this.OrderDate = orderDate;
            this.Domain = domain;
            this.Administrator = administrator;
            this.Customer = customer;
            this.OrderNumber = orderNumber;
        }

        [NotNull]
        public TextFieldSettings OrderNumber { get; private set;}
    
        [NotNull]
        public TextFieldSettings Customer { get; private set;}
    
        [NotNull]
        public FieldSettings Administrator { get; private set; }
    
        [NotNull]
        public FieldSettings Domain { get; private set; }
    
        [NotNull]
        public FieldSettings OrderDate { get; private set; }     
    }
}