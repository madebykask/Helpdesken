namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.ComponentModel;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class ProjectLogEditModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(1000)]
        [DisplayName("Log Note")]
        public string LogText { get; set; }

        public int ResponsibleUserId { get; set; }
    }
}