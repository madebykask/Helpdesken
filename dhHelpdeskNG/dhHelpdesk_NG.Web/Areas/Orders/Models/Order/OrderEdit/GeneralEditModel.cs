namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public sealed class GeneralEditModel
    {
        public GeneralEditModel()
        {
        }

        public GeneralEditModel(
            ConfigurableFieldModel<int> orderNumber,
            ConfigurableFieldModel<string> customer,
            ConfigurableFieldModel<SelectList> administrator,
            ConfigurableFieldModel<SelectList> domain,
            ConfigurableFieldModel<DateTime?> orderDate, 
            string orderTypeName)
        {
            this.OrderTypeName = orderTypeName;
            this.OrderNumber = orderNumber;
            this.Customer = customer;
            this.Administrator = administrator;
            this.Domain = domain;
            this.OrderDate = orderDate;
        }

        public string OrderTypeName { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<int> OrderNumber { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> Customer { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<SelectList> Administrator { get; set; } 

        [IsId]
        public int? AdministratorId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Domain { get; set; } 

        [IsId]
        public int? DomainId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> OrderDate { get; set; }

        public static GeneralEditModel CreateEmpty()
        {
            return new GeneralEditModel(
                ConfigurableFieldModel<int>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<SelectList>.CreateUnshowable(),
                ConfigurableFieldModel<SelectList>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                string.Empty);
        }

        public bool HasShowableFields()
        {
            return this.OrderNumber.Show ||
                this.Customer.Show ||
                this.Administrator.Show ||
                this.Domain.Show ||
                this.OrderDate.Show;
        }
    }
}