namespace DH.Helpdesk.Mobile.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.Domain;

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