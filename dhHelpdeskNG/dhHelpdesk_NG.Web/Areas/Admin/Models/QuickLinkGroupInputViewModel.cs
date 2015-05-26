using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    public class QuickLinkGroupInputViewModel
    {
        public LinkGroup LinkGroup { get; set; }
        public Customer Customer { get; set; }
    }
}