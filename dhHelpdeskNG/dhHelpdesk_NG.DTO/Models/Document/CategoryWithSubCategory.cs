using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Common.Input;

namespace DH.Helpdesk.BusinessData.Models.Document
{
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
