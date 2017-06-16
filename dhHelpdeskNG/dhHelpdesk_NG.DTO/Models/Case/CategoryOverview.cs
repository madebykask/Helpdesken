using System;

namespace DH.Helpdesk.BusinessData.Models.Case.CaseHistory
{
    public class CategoryOverview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryGUID { get; set; }
    }
}