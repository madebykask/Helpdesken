namespace DH.Helpdesk.BusinessData.Models.Licenses.Products
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductModel : EntityBusinessModel
    {
        public ProductModel(
                int id,
                string productName, 
                int? manufacturerId, 
                int customerId, 
                DateTime createdDate, 
                DateTime changedDate, 
                ItemOverview[] applications)
        {
            this.Applications = applications;
            this.Id = id;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.CustomerId = customerId;
            this.ManufacturerId = manufacturerId;
            this.ProductName = productName;
        }

        public ProductModel(
                int id,
                string productName, 
                int? manufacturerId, 
                int customerId, 
                ItemOverview[] applications)
        {
            this.Applications = applications;
            this.Id = id;
            this.CustomerId = customerId;
            this.ManufacturerId = manufacturerId;
            this.ProductName = productName;
        }

        public ProductModel(int customerId)
        {
            this.CustomerId = customerId;
            this.Applications = new ItemOverview[0];
        }

        [NotNullAndEmpty]
        [MaxLength(50)]
        public string ProductName { get; private set; }

        [IsId]
        public int? ManufacturerId { get; private set; }

        [IsId]
        public int CustomerId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public ItemOverview[] Applications { get; private set; }

        public static ProductModel CreateDefault(int customerId)
        {
            return new ProductModel(customerId);
        }
    }
}