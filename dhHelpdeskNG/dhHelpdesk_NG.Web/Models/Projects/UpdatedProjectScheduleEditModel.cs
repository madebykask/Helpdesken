namespace dhHelpdesk_NG.Web.Models.Projects
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    public class UpdatedProjectScheduleEditModel
    {
        public UpdatedProjectScheduleEditModel()
        {
            this.Users = new List<SelectListItem>();
            this.Positions = Enumerable.Range(0, 99).Select(x => new SelectListItem { Value = x.ToString(), Text = x.ToString() }).ToList();
        }

        public ProjectScheduleEditModel ProjectShedule { get; set; }

        public List<SelectListItem> Users { get; set; }

        public List<SelectListItem> Positions { get; private set; }
    }
}