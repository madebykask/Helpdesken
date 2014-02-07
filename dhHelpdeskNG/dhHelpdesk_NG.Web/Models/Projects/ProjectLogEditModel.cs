namespace DH.Helpdesk.Web.Models.Projects
{
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class ProjectLogEditModel
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(1000)]
        [LocalizedDisplay("Log Note")]
        public string LogText { get; set; }

        public int ResponsibleUserId { get; set; }
    }
}