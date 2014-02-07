namespace DH.Helpdesk.BusinessData.Models.Faq.Output
{
    using System.Collections.Generic;

    public sealed class CategoryWithSubcategories
    {
        public CategoryWithSubcategories()
        {
            this.Subcategories = new List<CategoryWithSubcategories>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<CategoryWithSubcategories> Subcategories { get; set; }
    }
}
