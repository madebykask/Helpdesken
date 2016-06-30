namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class GeneralFieldSettingsModel
    {
        public GeneralFieldSettingsModel()
        {            
        }

        public GeneralFieldSettingsModel(
                FieldSettingsModel orderNumber, 
                FieldSettingsModel customer, 
                TextFieldSettingsModel administrator, 
                TextFieldSettingsModel domain,
                TextFieldSettingsModel orderDate,
                TextFieldSettingsModel status)
        {
            this.OrderDate = orderDate;
            this.Domain = domain;
            this.Administrator = administrator;
            this.Customer = customer;
            this.OrderNumber = orderNumber;
            this.Status = status;
        }

        [NotNull]
        [LocalizedDisplay("Ordernummer")]
        public FieldSettingsModel OrderNumber { get; set;}
    
        [NotNull]
        [LocalizedDisplay("Kund")]
        public FieldSettingsModel Customer { get; set;}
    
        [NotNull]
        [LocalizedDisplay("Handläggare")]
        public TextFieldSettingsModel Administrator { get; set; }
    
        [NotNull]
        [LocalizedDisplay("Domän")]
        public TextFieldSettingsModel Domain { get; set; }
    
        [NotNull]
        [LocalizedDisplay("Beställningsdatum")]
        public TextFieldSettingsModel OrderDate { get; set; }

        [NotNull]
        [LocalizedDisplay("Status")]
        public TextFieldSettingsModel Status { get; set; }   
    }
}