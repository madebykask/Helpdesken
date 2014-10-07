namespace DH.Helpdesk.BusinessData.Models.Licenses.Manufacturers
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ManufacturerModel : EntityBusinessModel
    {
        public ManufacturerModel(
                int customerId, 
                string manufacturerName, 
                DateTime createdDate, 
                DateTime changedDate)
        {
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.ManufacturerName = manufacturerName;
            this.CustomerId = customerId;
        }

        public ManufacturerModel(
                int customerId, 
                string manufacturerName)
        {
            this.ManufacturerName = manufacturerName;
            this.CustomerId = customerId;
        }

        public int CustomerId { get; private set; }

        [NotNullAndEmpty]
        [MaxLength(100)]
        public string ManufacturerName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }
    }
}