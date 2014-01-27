namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.ComponentModel.DataAnnotations;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class ProjectScheduleEditModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [LocalizedDisplay("Responsible")]
        public int? UserId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Sub Project")]
        public string Name { get; set; }

        [Range(0, 99)]
        [LocalizedDisplay("Pos")]
        public int Position { get; set; }

        [LocalizedDisplay("State")]
        public ScheduleStates State { get; set; }

        [Range(0, 99999)]
        [LocalizedDisplay("Time")]
        public int Time { get; set; }

        [LocalizedStringLength(1000)]
        [LocalizedDisplay("Description")]
        public string Description { get; set; }

        [LocalizedDisplay("Start Date")]
        public string StartDate { get; set; }

        [LocalizedDisplay("Finish Date")]
        public string FinishDate { get; set; }

        [LocalizedDisplay("Case")]
        public double? CaseNumber { get; set; }
    }
}