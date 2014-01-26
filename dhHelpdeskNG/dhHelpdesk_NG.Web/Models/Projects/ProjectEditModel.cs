namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class ProjectEditModel
    {
        public ProjectEditModel()
        {
            this.ProjectCollaboratorIds = new List<int>();
        }

        public int Id { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(50)]
        [DisplayName("Project Name")]
        public string Name { get; set; }

        public int? ProjectManagerId { get; set; }

        public int IsActive { get; set; }

        [LocalizedRequired]
        [LocalizedStringLength(2000)]
        [DisplayName("Project Description")]
        public string Description { get; set; }

        public string EndDate { get; set; }

        public List<int> ProjectCollaboratorIds { get; set; }
    }
}
