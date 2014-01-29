namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class NewProjectScheduleEditModel
    {
        public NewProjectScheduleEditModel()
        {
            this.Positions = Enumerable.Range(0, 100).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
        }

        public ProjectScheduleEditModel ProjectScheduleEditModel { get; set; }

        public List<SelectListItem> Positions { get; private set; }
    }
}