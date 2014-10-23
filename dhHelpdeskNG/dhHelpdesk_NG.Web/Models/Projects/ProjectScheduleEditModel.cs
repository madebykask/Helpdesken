namespace DH.Helpdesk.Web.Models.Projects
{
    using System.ComponentModel.DataAnnotations;

    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ProjectScheduleEditModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [LocalizedDisplay("Ansvarig")]
        public int? UserId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Delprojekt")]
        public string Name { get; set; }

        [Range(0, 99)]
        [LocalizedDisplay("Pos")]
        public int Position { get; set; }

        [LocalizedDisplay("Status")]
        public ScheduleStates? State { get; set; }

        [Range(0, 99999)]
        [LocalizedDisplay("Tid")]
        public int Time { get; set; }

        [LocalizedStringLength(1000)]
        [LocalizedDisplay("Beskrivning")]
        public string Description { get; set; }

        [LocalizedDisplay("Startdatum")]
        public string StartDate { get; set; }

        [LocalizedDisplay("Slutdatum")]
        public string FinishDate { get; set; }

        [LocalizedDisplay("Ärende")]
        public decimal? CaseNumber { get; set; }
    }
}