namespace DH.Helpdesk.BusinessData.Models.FinishingCause
{
    using System.Collections.Generic;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FinishingCauseItem
    {
         public FinishingCauseItem(
                int id, 
                int? parentId, 
                string name)
        {
            this.Name = name;
            this.ParentId = parentId;
            this.Id = id;
            this.Children = new List<FinishingCauseItem>();
        }

          public FinishingCauseItem()
        {
            this.Children = new List<FinishingCauseItem>();
        }

        [IsId]
        public int Id { get; set; }

        [IsId]
        public int? ParentId { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public List<FinishingCauseItem> Children { get; set; }

        public FinishingCauseItem Parent { get; set; }

        public byte GetLevel()
        {
            byte level = 0;
            var parent = this.Parent;
            while (parent != null)
            {
                parent = parent.Parent;
                level++;
            }

            return level;
        }
    }
}