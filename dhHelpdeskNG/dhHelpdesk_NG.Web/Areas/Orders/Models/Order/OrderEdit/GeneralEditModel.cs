namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System;
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using FieldModels;

    public sealed class GeneralEditModel
    {
        public GeneralEditModel()
        {
        }

        public GeneralEditModel(
            ConfigurableFieldModel<int> orderNumber,
            ConfigurableFieldModel<string> customer,
            ConfigurableFieldModel<int?> administrator,
            ConfigurableFieldModel<int?> domain,
            ConfigurableFieldModel<DateTime?> orderDate,
            string orderTypeName,
            ConfigurableFieldModel<SelectList> status)
        {
            OrderTypeName = orderTypeName;
            OrderNumber = orderNumber;
            Customer = customer;
            Administrator = administrator;
            Domain = domain;
            OrderDate = orderDate;
            Status = status;
        }

        public string Header { get; set; }

        public string OrderTypeName { get; private set; }

        [NotNull]
        public ConfigurableFieldModel<int> OrderNumber { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> Customer { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<int?> Administrator { get; set; } 


        [NotNull]
        public ConfigurableFieldModel<int?> Domain { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<DateTime?> OrderDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Status { get; set; }

        [IsId]
        public int? StatusId { get; set; }

        [NotNull]
        public SelectList Administrators { get; set; }

        [NotNull]
        public SelectList Domains { get; set; }

        public static GeneralEditModel CreateEmpty()
        {
            return new GeneralEditModel(
                ConfigurableFieldModel<int>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<DateTime?>.CreateUnshowable(),
                string.Empty,
                ConfigurableFieldModel<SelectList>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return OrderNumber.Show ||
                Customer.Show ||
                Administrator.Show ||
                Domain.Show ||
                OrderDate.Show ||
                Status.Show;
        }
    }
}