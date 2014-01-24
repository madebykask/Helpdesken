namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class ProjectScheduleEditModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int? UserId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [DisplayName("ProjectShedule Name")]
        public string Name { get; set; }

        [Range(0, 99)]
        public int Position { get; set; }

        [Range(0, 3)]
        public int State { get; set; }

        [Range(0, 99999)]
        public int Time { get; set; }

        [LocalizedStringLength(1000)]
        [DisplayName("ProjectShedule Description")]
        public string Description { get; set; }

        public string StartDate { get; set; }

        public string FinishDate { get; set; }

        public double? CaseNumber { get; set; }
    }
}