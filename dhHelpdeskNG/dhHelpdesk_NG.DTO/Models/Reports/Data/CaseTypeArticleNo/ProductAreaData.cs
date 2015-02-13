namespace DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.ProductArea;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class ProductAreaData
    {
        public ProductAreaData(
            ProductAreaItem productArea, 
            List<CasesData> cases)
        {
            this.Cases = cases;
            this.ProductArea = productArea;
            this.Children = new List<ProductAreaData>();
        }

        [NotNull]
        public ProductAreaItem ProductArea { get; private set; }

        [NotNull]
        public List<CasesData> Cases { get; private set; }

        public ProductAreaData Parent { get; set; }

        [NotNull]
        public List<ProductAreaData> Children { get; set; } 

        public int GetTotal()
        {
            return this.Cases.Sum(c => c.Number);
        }

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