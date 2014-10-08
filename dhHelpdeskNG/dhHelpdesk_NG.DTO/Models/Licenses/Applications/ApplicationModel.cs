namespace DH.Helpdesk.BusinessData.Models.Licenses.Applications
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ApplicationModel : EntityBusinessModel
    {
        public ApplicationModel(
                int id,
                int customerId, 
                string applicationName, 
                DateTime createdDate, 
                DateTime changedDate)
        {
            this.Id = id;
            this.ChangedDate = changedDate;
            this.CreatedDate = createdDate;
            this.ApplicationName = applicationName;
            this.CustomerId = customerId;
        }

        public ApplicationModel(
                int id,
                int customerId, 
                string applicationName)
        {
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

        public static ApplicationModel CreateDefault(int customerId)
        {
            return new ApplicationModel(customerId);
        }
    }
}