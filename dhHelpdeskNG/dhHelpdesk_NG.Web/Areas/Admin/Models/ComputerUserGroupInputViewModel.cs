using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Areas.Admin.Models
{
    public class ComputerUserGroupInputViewModel
    {
        public ComputerUserGroup ComputerUserGroup { get; set; }

        public IList<SelectListItem> TypeChoices { get; set; }
        public IList<SelectListItem> OUsAvailable { get; set; }
        public IList<SelectListItem> OUsSelected { get; set; }

        public ComputerUserGroupInputViewModel() { }
    }
}