namespace DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

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
            ConfigurableFieldModel<SelectList> department,
            ConfigurableFieldModel<SelectList> unit,
            ConfigurableFieldModel<string> ordererAddress,
            ConfigurableFieldModel<string> ordererInvoiceAddress,
            ConfigurableFieldModel<string> ordererReferenceNumber,
            ConfigurableFieldModel<string> accountingDimension1,
            ConfigurableFieldModel<string> accountingDimension2,
            ConfigurableFieldModel<string> accountingDimension3,
            ConfigurableFieldModel<string> accountingDimension4,
            ConfigurableFieldModel<string> accountingDimension5)
        {
            this.OrdererId = ordererId;
            this.OrdererName = ordererName;
            this.OrdererLocation = ordererLocation;
            this.OrdererEmail = ordererEmail;
            this.OrdererPhone = ordererPhone;
            this.OrdererCode = ordererCode;
            this.Department = department;
            this.Unit = unit;
            this.OrdererAddress = ordererAddress;
            this.OrdererInvoiceAddress = ordererInvoiceAddress;
            this.OrdererReferenceNumber = ordererReferenceNumber;
            this.AccountingDimension1 = accountingDimension1;
            this.AccountingDimension2 = accountingDimension2;
            this.AccountingDimension3 = accountingDimension3;
            this.AccountingDimension4 = accountingDimension4;
            this.AccountingDimension5 = accountingDimension5;
        }

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
        public ConfigurableFieldModel<SelectList> Department { get; set; } 

        [IsId]
        public int? DepartmentId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<SelectList> Unit { get; set; } 

        [IsId]
        public int? UnitId { get; set; }

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
    }
}