using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DH.Helpdesk.Web.Models.Error
{
    public class HandleErrorInfoGuid : HandleErrorInfo
    {
        public HandleErrorInfoGuid(Exception ex, string currentController, string currentAction, Guid guid)
            : base(ex, currentController, currentAction)
        {
            Guid = guid;
        }

        public Guid Guid { get; set; }
    }
}