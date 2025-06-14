﻿namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class OUIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<OU> OUs { get; set; }
        public bool IsShowOnlyActive { get; set; }
    }
}