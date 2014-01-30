namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class ProjectEditModel
    {
        public ProjectEditModel()
        {
            this.ProjectCollaboratorIds = new List<int>();
        }

        [LocalizedDisplay("Project Number")]
        public int Id { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [LocalizedDisplay("Project Name")]
        public string Name { get; set; }

        [LocalizedDisplay("Project Manager")]
        public int? ProjectManagerId { get; set; }

        [LocalizedDisplay("State")]
        public bool IsActive { get; set; }

        [LocalizedStringLength(2000)]
        [LocalizedDisplay("Project Description")]
        public string Description { get; set; }

        [LocalizedDisplay("Project Date")]
        public string StartDate { get; set; }

        [LocalizedDisplay("Finishing Date")]
        public string EndDate { get; set; }

        [LocalizedDisplay("Project members")]
        public List<int> ProjectCollaboratorIds { get; set; }

        [LocalizedDisplay("Project files")]
        public List<int> ProjectFileNames { get; set; }
    }
}
