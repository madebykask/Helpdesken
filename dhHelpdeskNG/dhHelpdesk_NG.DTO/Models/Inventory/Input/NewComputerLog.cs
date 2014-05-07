namespace DH.Helpdesk.BusinessData.Models.Inventory.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Attributes;
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ComputerLog : BusinessModel
    {
        private ComputerLog(
            ModelStates modelState,
            int computerId,
            int? createdByUserId,
            string computerLogCategory,
            string computerLogText,
            DateTime createdDate)
        {
            this.State = modelState;
            this.ComputerId = computerId;
            this.CreatedByUserId = createdByUserId;
            this.ComputerLogCategory = computerLogCategory;
            this.ComputerLogText = computerLogText;
            this.CreatedDate = createdDate;
        }

        [IsId]
        public int ComputerId { get; private set; }

        [IsId]
        public int? CreatedByUserId { get; private set; }

        [NotNull]
        public string ComputerLogCategory { get; private set; }

        [NotNull]
        public string ComputerLogText { get; private set; }

        [AllowRead(ModelStates.Created)]
        public DateTime CreatedDate { get; private set; }

        public static ComputerLog CreateNew(
            int computerId,
            int? createdByUserId,
            string computerLogCategory,
            string computerLogText,
            DateTime createdDate)
        {
            return new ComputerLog(
                ModelStates.Created,
                computerId,
                createdByUserId,
                computerLogCategory,
                computerLogText,
                createdDate);
        }
    }
}