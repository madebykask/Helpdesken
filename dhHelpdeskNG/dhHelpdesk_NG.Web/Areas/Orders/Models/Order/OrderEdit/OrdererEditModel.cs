using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using FieldModels;

    public sealed class OrdererEditModel
    {
        public OrdererEditModel()
        {            
        }

        public OrdererEditModel(
            ConfigurableFieldModel<string> ordererId,
            ConfigurableFieldModel<string> ordererName,
            ConfigurableFieldModel<string> ordererLocation,
            ConfigurableFieldModel<string> ordererEmail,
            ConfigurableFieldModel<string> ordererPhone,
            ConfigurableFieldModel<string> ordererCode,
            ConfigurableFieldModel<int?> department,
            ConfigurableFieldModel<int?> unit,
            ConfigurableFieldModel<string> ordererAddress,
            ConfigurableFieldModel<string> ordererInvoiceAddress,
            ConfigurableFieldModel<string> ordererReferenceNumber,
            ConfigurableFieldModel<string> accountingDimension1,
            ConfigurableFieldModel<string> accountingDimension2,
            ConfigurableFieldModel<string> accountingDimension3,
            ConfigurableFieldModel<string> accountingDimension4,
            ConfigurableFieldModel<string> accountingDimension5)
        {
            OrdererId = ordererId;
            OrdererName = ordererName;
            OrdererLocation = ordererLocation;
            OrdererEmail = ordererEmail;
            OrdererPhone = ordererPhone;
            OrdererCode = ordererCode;
            Department = department;
            Unit = unit;
            OrdererAddress = ordererAddress;
            OrdererInvoiceAddress = ordererInvoiceAddress;
            OrdererReferenceNumber = ordererReferenceNumber;
            AccountingDimension1 = accountingDimension1;
            AccountingDimension2 = accountingDimension2;
            AccountingDimension3 = accountingDimension3;
            AccountingDimension4 = accountingDimension4;
            AccountingDimension5 = accountingDimension5;
        }

        public string Header { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> OrdererId { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererName { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererLocation { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererEmail { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererPhone { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererCode { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<int?> Department { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<int?> Unit { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererAddress { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererInvoiceAddress { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> OrdererReferenceNumber { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension1 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension2 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension3 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension4 { get; set; } 

        [NotNull]
        public ConfigurableFieldModel<string> AccountingDimension5 { get; set; }

        [NotNull]
        public SelectList Departments { get; set; }

        [NotNull]
        public SelectList Units { get; set; }

        public static OrdererEditModel CreateEmpty()
        {
            return new OrdererEditModel(
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<int?>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable(),
                ConfigurableFieldModel<string>.CreateUnshowable());
        }

        public bool HasShowableFields()
        {
            return OrdererId.Show ||
                OrdererName.Show ||
                OrdererLocation.Show ||
                OrdererEmail.Show ||
                OrdererPhone.Show ||
                OrdererCode.Show ||
                Department.Show ||
                Unit.Show ||
                OrdererAddress.Show ||
                OrdererInvoiceAddress.Show ||
                OrdererReferenceNumber.Show ||
                AccountingDimension1.Show ||
                AccountingDimension2.Show ||
                AccountingDimension3.Show ||
                AccountingDimension4.Show ||
                AccountingDimension5.Show;
        }
    }
}