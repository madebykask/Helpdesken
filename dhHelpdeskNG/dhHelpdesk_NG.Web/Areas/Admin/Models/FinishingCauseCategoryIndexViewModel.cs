﻿namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class FinishingCauseCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<FinishingCauseCategory> FinishingCauseCategories { get; set; }
    }
}