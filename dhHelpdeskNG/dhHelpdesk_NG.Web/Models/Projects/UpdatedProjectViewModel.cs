namespace DH.Helpdesk.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Projects.Output;
    using DH.Helpdesk.Web.Models.Case;

    public class UpdatedProjectViewModel
    {
        public int Guid { get; set; }

        public ProjectEditModel ProjectEditModel { get; set; }

        public MultiSelectList Users { get; set; }

        public UpdatedProjectScheduleEditModel UpdatedProjectScheduleEditModel { get; set; }

        public ProjectLogEditModel ProjectLog { get; set; }

        public List<ProjectLogOverview> ProjectLogs { get; set; }

        public List<CaseOverview> CaseOverviews { get; set; }

        public NewProjectScheduleEditModel NewProjectScheduleEditModel { get; set; }

		public List<string> FileUploadWhiteList { get; set; }
	}
}