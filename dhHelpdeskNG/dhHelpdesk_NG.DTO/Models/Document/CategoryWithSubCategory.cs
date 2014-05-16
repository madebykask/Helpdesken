using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Document
{
    using DH.Helpdesk.BusinessData.Models.Shared.Input;

    public sealed class CategoryWithSubCategory :INewBusinessModel
    {
        public CategoryWithSubCategory()
        {
            this.Subcategories = new List<CategoryWithSubCategory>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int UniqueId { get; set; }

        public List<CategoryWithSubCategory> Subcategories { get; set; }
    }
}
