namespace DH.Helpdesk.BusinessData.Models.Licenses.Applications
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ApplicationModel : Shared.Input.BusinessModel
    {
        public ApplicationModel(
                int id,
                int customerId, 
                string applicationName, 
                int? productId,
                DateTime createdDate, 
                DateTime changedDate)
        {
            this.ProductId = productId;
            this.Id = id;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.ApplicationName = applicationName;
            this.CustomerId = customerId;
        }

        public ApplicationModel(
                int id,
                int customerId, 
                string applicationName, 
                int? productId)
        {
            this.ProductId = productId;
            this.Id = id;
            this.ApplicationName = applicationName;
            this.CustomerId = customerId;
        }

        public ApplicationModel(int customerId)
        {
            this.CustomerId = customerId;
        }

        [IsId]
        public int CustomerId { get; private set; }

        [NotNullAndEmpty]
        [MaxLength(100)]
        public string ApplicationName { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime ChangedDate { get; private set; }

        public int? ProductId { get; private set; }

        public static ApplicationModel CreateDefault(int customerId)
        {
            return new ApplicationModel(customerId);
        }
    }
}