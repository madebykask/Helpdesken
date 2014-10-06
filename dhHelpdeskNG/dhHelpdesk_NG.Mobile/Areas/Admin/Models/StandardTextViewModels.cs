namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;

    using DH.Helpdesk.Domain;

    public class StandardTextIndexViewModel
    {
        public Customer Customer { get; set; }
        public IList<StandardText> StandardTexts { get; set; }
    }

    public class StandardTextInputViewModel
    {
        public Customer Customer { get; set; }
        public StandardText StandardText { get; set; }
    }
}