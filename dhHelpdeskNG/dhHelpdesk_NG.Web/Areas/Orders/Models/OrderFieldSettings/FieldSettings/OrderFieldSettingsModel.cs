using DH.Helpdesk.BusinessData.Enums.Orders.Fields;

namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class OrderFieldSettingsModel
    {
        public OrderFieldSettingsModel()
        {            
        }

        public OrderFieldSettingsModel(
                FieldSettingsModel property, 
                TextFieldSettingsModel orderRow1, 
                TextFieldSettingsModel orderRow2, 
                TextFieldSettingsModel orderRow3, 
                TextFieldSettingsModel orderRow4, 
                TextFieldSettingsModel orderRow5, 
                TextFieldSettingsModel orderRow6, 
                TextFieldSettingsModel orderRow7, 
                TextFieldSettingsModel orderRow8, 
                TextFieldSettingsModel configuration, 
                TextFieldSettingsModel orderInfo, 
                TextFieldSettingsModel orderInfo2)
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

        [LocalizedStringLength(50)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderProperty)]
        public FieldSettingsModel Property { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow1)]
        public TextFieldSettingsModel OrderRow1 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow2)]
        public TextFieldSettingsModel OrderRow2 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow3)]
        public TextFieldSettingsModel OrderRow3 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow4)]
        public TextFieldSettingsModel OrderRow4 { get; set; }
 
        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow5)]
        public TextFieldSettingsModel OrderRow5 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow6)]
        public TextFieldSettingsModel OrderRow6 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow7)]
        public TextFieldSettingsModel OrderRow7 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderRow8)]
        public TextFieldSettingsModel OrderRow8 { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderConfiguration)]
        public TextFieldSettingsModel Configuration { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderInfo)]
        public TextFieldSettingsModel OrderInfo { get; set; }

        [NotNull]
        [LocalizedDisplay(OrderLabels.OrderInfo2)]
        public TextFieldSettingsModel OrderInfo2 { get; set; }         
    }
}