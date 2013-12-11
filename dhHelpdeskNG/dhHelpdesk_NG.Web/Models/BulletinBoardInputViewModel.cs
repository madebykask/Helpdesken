using System.Collections.Generic;
using System.Web.Mvc;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Web.Models
{
    public class BulletinBoardIndexViewModel
    {
        public string SearchBbs { get; set; }

        public BulletinBoard BulletinBoard { get; set; }

        public IEnumerable<BulletinBoard> BulletinBoards { get; set; }
    }

    public class BulletinBoardInputViewModel
    {
        public BulletinBoard BulletinBoard { get; set; }

        public IList<SelectListItem> WGsAvailable { get; set; }
        public IList<SelectListItem> WGsSelected { get; set; }

        public BulletinBoardInputViewModel() { }
    }
}