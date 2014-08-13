namespace DH.Helpdesk.BusinessData.Models.Inventory
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerHistory
    {
        public ComputerHistory(int computerId, string userId, DateTime createdDate)
        {
            this.ComputerId = computerId;
            this.UserId = userId;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int ComputerId { get; private set; }

        [NotNullAndEmpty]
        public string UserId { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}