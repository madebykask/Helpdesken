namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Web.Mvc;

    public class ProjectEditViewModel
    {
        public ProjectEditModel ProjectEditModel { get; set; }

        public MultiSelectList Users { get; set; }
    }
}