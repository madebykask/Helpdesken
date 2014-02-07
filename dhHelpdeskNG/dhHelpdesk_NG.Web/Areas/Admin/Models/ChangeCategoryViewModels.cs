namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Changes;

    public class ChangeCategoryIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<ChangeCategoryEntity> ChangeCategories { get; set; }
    }

    public class ChangeCategoryInputViewModel
    {
        public Customer Customer { get; set; }
        public ChangeCategoryEntity ChangeCategory { get; set; }
    }
}