namespace DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ManufacturerModel : Shared.Input.BusinessModel
    {
        public ManufacturerModel(
                int id,
                int customerId, 
                string manufacturerName, 
                DateTime createdDate, 
                DateTime changedDate)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.ManufacturerName = manufacturerName;
            this.CustomerId = customerId;
        }

        public ManufacturerModel(
                int id,
                int customerId, 
                string manufacturerName)
        {
            this.Id = id;
            this.ManufacturerName = manufacturerName;
            this.CustomerId = customerId;
        }

        public ManufacturerModel(int customerId)
        {
            this.CustomerId = customerId;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [NotNullAndEmpty]
        [MaxLength(100)]
        public string ManufacturerName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public static ManufacturerModel CreateDefault(int customerId)
        {
            return new ManufacturerModel(customerId);
        }
    }
}