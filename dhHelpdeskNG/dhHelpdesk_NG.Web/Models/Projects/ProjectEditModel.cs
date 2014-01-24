namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Web.Mvc;

    using dhHelpdesk_NG.Web.Infrastructure.LocalizedAttributes;

    public class ProjectEditModel
    {
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

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public List<SelectListItem> ProjectCollaborators { get; set; }
    }
}
