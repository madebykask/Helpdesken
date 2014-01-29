namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Projects.Output;

    public class UpdatedProjectViewModel
    {
        public ProjectEditModel ProjectEditModel { get; set; }

        public MultiSelectList Users { get; set; }

        public UpdatedProjectScheduleEditModel UpdatedProjectScheduleEditModel { get; set; }

        public ProjectLogEditModel ProjectLog { get; set; }

        public List<ProjectLogOverview> ProjectLogs { get; set; }

        public List<CaseOverview> CaseOverviews { get; set; }

        public NewProjectScheduleEditModel NewProjectScheduleEditModel { get; set; }
    }
}