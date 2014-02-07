namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

    public class ComputerUserGroupInputViewModel
    {
        public ComputerUserGroup ComputerUserGroup { get; set; }

        public IList<SelectListItem> TypeChoices { get; set; }
        public IList<SelectListItem> OUsAvailable { get; set; }
        public IList<SelectListItem> OUsSelected { get; set; }

        public ComputerUserGroupInputViewModel() { }
    }
}