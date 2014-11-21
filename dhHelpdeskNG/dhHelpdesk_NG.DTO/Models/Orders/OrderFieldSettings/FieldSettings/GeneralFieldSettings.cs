namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralFieldSettings
    {
        public GeneralFieldSettings(
                FieldSettings orderNumber, 
                FieldSettings customer, 
                TextFieldSettings administrator, 
                TextFieldSettings domain, 
                TextFieldSettings orderDate)
        {
            this.OrderDate = orderDate;
            this.Domain = domain;
            this.Administrator = administrator;
            this.Customer = customer;
            this.OrderNumber = orderNumber;
        }

        [NotNull]
        public FieldSettings OrderNumber { get; private set;}
    
        [NotNull]
        public FieldSettings Customer { get; private set;}
    
        [NotNull]
        public TextFieldSettings Administrator { get; private set; }
    
        [NotNull]
        public TextFieldSettings Domain { get; private set; }
    
        [NotNull]
        public TextFieldSettings OrderDate { get; private set; }     
    }
}