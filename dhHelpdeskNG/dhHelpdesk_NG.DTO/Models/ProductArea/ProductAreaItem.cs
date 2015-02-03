namespace DH.Helpdesk.BusinessData.Models.ProductArea
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductAreaItem
    {
        public ProductAreaItem(
                int id, 
                int? parentId, 
                string name)
        {
            this.Name = name;
            this.ParentId = parentId;
            this.Id = id;
            this.Children = new List<ProductAreaItem>();
        }

        public ProductAreaItem()
        {
            this.Children = new List<ProductAreaItem>();
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? ParentId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public List<ProductAreaItem> Children { get; set; }

        public ProductAreaItem Parent { get; set; }
    }
}