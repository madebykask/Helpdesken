namespace DH.Helpdesk.BusinessData.Models.CaseType
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseTypeItem
    {
         public CaseTypeItem(
                int id, 
                int? parentId, 
                string name)
        {
            this.Name = name;
            this.ParentId = parentId;
            this.Id = id;
            this.Children = new List<CaseTypeItem>();
        }

        public CaseTypeItem()
        {
            this.Children = new List<CaseTypeItem>();
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? ParentId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public List<CaseTypeItem> Children { get; set; }

        public CaseTypeItem Parent { get; set; }
    }
}