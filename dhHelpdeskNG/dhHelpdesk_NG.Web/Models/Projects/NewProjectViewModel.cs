namespace DH.Helpdesk.Web.Models.Projects
{
	using System.Collections.Generic;
	using System.Web.Mvc;

	public class NewProjectViewModel
    {
        public string Guid { get; set; }

        public ProjectEditModel ProjectEditModel { get; set; }

		public List<string> FileUploadWhiteList { get; set; }

		public MultiSelectList Users { get; set; }
    }
}