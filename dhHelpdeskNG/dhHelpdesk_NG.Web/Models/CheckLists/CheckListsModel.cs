using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DH.Helpdesk.Web.Models.CheckLists
{
    public class CheckListsModel
    {

        public IList<CheckListServiceModel> InputServices { get; set; }

        public IList<CheckListActionsInputModel> InputActions { get; set; }

    }
}