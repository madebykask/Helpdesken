using System;
using System.Collections.Generic;


namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class CategoryOverview
    {
        public CategoryOverview()
        {
            SubCategories = new List<CategoryOverview>();    
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CategoryGUID { get; set; }
        public int IsActive { get; set; }

        public List<CategoryOverview> SubCategories { get; set; }
    }
}