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

        [LocalizedStringLength(30)]
        public string Header { get; set; }

        [NotNull]
        [LocalizedDisplay("Egenskap")]
        public FieldSettingsModel Property { get; set; }

        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow1 { get; set; }

        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow2 { get; set; }

        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow3 { get; set; }

        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow4 { get; set; }
 
        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow5 { get; set; }

        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow6 { get; set; }

        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow7 { get; set; }

        [NotNull]
        [LocalizedDisplay("Beställning rad")]
        public TextFieldSettingsModel OrderRow8 { get; set; }

        [NotNull]
        [LocalizedDisplay("Konfigurering")]
        public TextFieldSettingsModel Configuration { get; set; }

        [NotNull]
        [LocalizedDisplay("Övrigt")]
        public TextFieldSettingsModel OrderInfo { get; set; }

        [NotNull]
        [LocalizedDisplay("Info")]
        public TextFieldSettingsModel OrderInfo2 { get; set; }         
    }
}