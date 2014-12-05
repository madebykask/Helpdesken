namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class OrderEditModel
    {
        public OrderEditModel()
        {            
        }

        public OrderEditModel(
            ConfigurableFieldModel<SelectList> property,
            ConfigurableFieldModel<string> orderRow1,
            ConfigurableFieldModel<string> orderRow2,
            ConfigurableFieldModel<string> orderRow3,
            ConfigurableFieldModel<string> orderRow4,
            ConfigurableFieldModel<string> orderRow5,
            ConfigurableFieldModel<string> orderRow6,
            ConfigurableFieldModel<string> orderRow7,
            ConfigurableFieldModel<string> orderRow8,
            ConfigurableFieldModel<string> configuration,
            ConfigurableFieldModel<string> orderInfo,
            ConfigurableFieldModel<string> orderInfo2)
        {
            this.Property = property;
            this.OrderRow1 = orderRow1;
            this.OrderRow2 = orderRow2;
            this.OrderRow3 = orderRow3;
            this.OrderRow4 = orderRow4;
            this.OrderRow5 = orderRow5;
            this.OrderRow6 = orderRow6;
            this.OrderRow7 = orderRow7;
            this.OrderRow8 = orderRow8;
            this.Configuration = configuration;
            this.OrderInfo = orderInfo;
            this.OrderInfo2 = orderInfo2;
        }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Property { get; set; } 

        [IsId]
        public int? PropertyId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow1 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow2 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow3 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow4 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow5 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow6 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow7 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderRow8 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> Configuration { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderInfo { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrderInfo2 { get; set; }

        public bool HasShowableFields()
        {
            return this.Property.Show ||
                this.OrderRow1.Show ||
                this.OrderRow2.Show ||
                this.OrderRow3.Show ||
                this.OrderRow4.Show ||
                this.OrderRow5.Show ||
                this.OrderRow6.Show ||
                this.OrderRow7.Show ||
                this.OrderRow8.Show ||
                this.Configuration.Show ||
                this.OrderInfo.Show ||
                this.OrderInfo2.Show;
        }
    }
}