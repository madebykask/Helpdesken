namespace DH.Helpdesk.BusinessData.Models.Inventory.Output
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerLogOverview
    {
        public ComputerLogOverview(int id, int computerId, string createdByUserName, string computerLogText, DateTime createdDate)
        {
            this.Id = id;
            this.ComputerId = computerId;
            this.CreatedByUserName = createdByUserName;
            this.ComputerLogText = computerLogText;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ComputerId { get; set; }

        [IsId]
        public string CreatedByUserName { get; set; }

        [NotNullAndEmpty]
        public string ComputerLogText { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}