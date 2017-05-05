using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

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

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.GeneralOrderNumber)]
        public FieldSettingsModel OrderNumber { get; set;}
    
        [NotNull]
        [LocalizedDisplay(OrderLabels.GeneralCustomer)]
        public FieldSettingsModel Customer { get; set;}
    
        [NotNull]
        [LocalizedDisplay(OrderLabels.GeneralAdministrator)]
        public TextFieldSettingsModel Administrator { get; set; }
    
        [NotNull]
        [LocalizedDisplay(OrderLabels.GeneralDomain)]
        public TextFieldSettingsModel Domain { get; set; }
    
        [NotNull]
        [LocalizedDisplay(OrderLabels.GeneralOrderDate)]
        public TextFieldSettingsModel OrderDate { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.GeneralStatus)]
        public TextFieldSettingsModel Status { get; set; }   
    }
}