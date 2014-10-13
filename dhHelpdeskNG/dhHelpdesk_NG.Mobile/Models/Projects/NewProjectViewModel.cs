namespace DH.Helpdesk.Mobile.Models.Projects
{
    using System.Web.Mvc;

    public class NewProjectViewModel
    {
        public string Guid { get; set; }

        public ProjectEditModel ProjectEditModel { get; set; }

        public MultiSelectList Users { get; set; }
    }
}