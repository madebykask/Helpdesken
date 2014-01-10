namespace dhHelpdesk_NG.DTO.DTOs.FinishingCause
{
    using System.Collections.Generic;

    public sealed class FinishingCauseOverview
    {
        public FinishingCauseOverview()
        {
            this.ChildFinishingCauses = new List<FinishingCauseOverview>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<FinishingCauseOverview> ChildFinishingCauses { get; set; }
    }
}
