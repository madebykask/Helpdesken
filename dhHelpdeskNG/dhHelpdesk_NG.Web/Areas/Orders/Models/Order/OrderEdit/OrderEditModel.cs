namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Web.Mvc;

    using Common.ValidationAttributes;
    using FieldModels;

    public sealed class OrderEditModel
    {
        public OrderEditModel()
        {            
        }

        public OrderEditModel(
            ConfigurableFieldModel<int?> property,
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
            ConfigurableFieldModel<int> orderInfo2)
        {
            Property = property;
            OrderRow1 = orderRow1;
            OrderRow2 = orderRow2;
            OrderRow3 = orderRow3;
            OrderRow4 = orderRow4;
            OrderRow5 = orderRow5;
            OrderRow6 = orderRow6;
            OrderRow7 = orderRow7;
            OrderRow8 = orderRow8;
            Configuration = configuration;
            OrderInfo = orderInfo;
            OrderInfo2 = orderInfo2;
        }

        [NotNull]
        public ConfigurableFieldModel<int?> Property { get; set; } 

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
        public ConfigurableFieldModel<int> OrderInfo2 { get; set; }

        [NotNull]
        public SelectList Properties { get; set; }

        public static OrderEditModel CreateEmpty()
        {
            return new OrderEditModel(
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<int>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return Property.Show ||
                OrderRow1.Show ||
                OrderRow2.Show ||
                OrderRow3.Show ||
                OrderRow4.Show ||
                OrderRow5.Show ||
                OrderRow6.Show ||
                OrderRow7.Show ||
                OrderRow8.Show ||
                Configuration.Show ||
                OrderInfo.Show ||
                OrderInfo2.Show;
        }
    }
}