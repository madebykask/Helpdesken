namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Common.Input;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class NewComputerLog : INewBusinessModel
    {
        public NewComputerLog(int computerId, int? createdByUserId, string computerLogCategory, string computerLogText, DateTime createdDate)
        {
            this.ComputerId = computerId;
            this.CreatedByUserId = createdByUserId;
            this.ComputerLogCategory = computerLogCategory;
            this.ComputerLogText = computerLogText;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int ComputerId { get; set; }

        [IsId]
        public int? CreatedByUserId { get; set; }

        [NotNullAndEmpty]
        public string ComputerLogCategory { get; set; }

        [NotNullAndEmpty]
        public string ComputerLogText { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}