using System;

namespace dhHelpdesk_NG.DTO.DTOs.Problem.Output
{
    using dhHelpdesk_NG.Common.ValidationAttributes;

    public class ProblemOverview
    {
        [IsId]
        public int Id { get; set; }

        public string Name { get; set; }

        public int ProblemNumber { get; set; }

        public string Description { get; set; }

        public string ResponsibleUserName { get; set; }

        public DateTime? FinishingDate { get; set; }

        public int? ResponsibleUserId { get; set; }

        public string InventoryNumber { get; set; }

        public bool ShowOnStartPage { get; set; }

        public bool IsExistConnectedCases { get; set; }
    }
}
