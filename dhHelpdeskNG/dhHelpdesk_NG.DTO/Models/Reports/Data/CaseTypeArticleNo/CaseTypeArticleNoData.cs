namespace DH.Helpdesk.BusinessData.Models.Reports.Data.CaseTypeArticleNo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class CaseTypeArticleNoData
    {
        public CaseTypeArticleNoData(
                List<CaseTypeData> caseTypes, 
                List<ProductAreaData> productAreas)
        {
            this.ProductAreas = productAreas;
            this.CaseTypes = caseTypes;
        }

        [NotNull]
        public List<CaseTypeData> CaseTypes { get; private set; }
 
        [NotNull]
        public List<ProductAreaData> ProductAreas { get; private set; }

        public double GetTotalPercentsForCaseType(CasesData cases, CaseTypeData caseType)
        {
            var total = this.GetTotalForCaseType(caseType);
            if (total == 0)
            {
                return 0;
            }

            return Math.Round(((double)cases.Number / total) * 100, 1);
        }

        public double GetTotalPercentsForProductArea(ProductAreaData productArea)
        {
            var total = this.GetTotal();
            if (total == 0)
            {
                return 0;
            }

            return Math.Round(((double)productArea.GetTotal() / total) * 100, 1);
        }

        public int GetTotal()
        {
            var total = 0;
            foreach (var productArea in this.ProductAreas)
            {
                this.CalcTotal(productArea, ref total);
            }

            return total;
        }

        public int GetTotalForCaseType(CaseTypeData caseType)
        {
            var total = 0;
            foreach (var productArea in this.ProductAreas)
            {
                this.CalcTotalForCaseType(productArea, caseType, ref total);
            }

            return total;
        }

        public List<ProductAreaData> GetLineProductAreas()
        {
            var productAreas = new List<ProductAreaData>();
            foreach (var productArea in this.ProductAreas)
            {
                this.AddLineProductAreas(productArea, productAreas);
            }

            return productAreas;
        }

        public bool HasCaseTypeDetails()
        {
            return this.CaseTypes.Any(t => !string.IsNullOrEmpty(t.Details));
        }

        private void AddLineProductAreas(ProductAreaData productArea, List<ProductAreaData> productAreas)
        {
            productAreas.Add(productArea);
            foreach (var child in productArea.Children)
            {
                this.AddLineProductAreas(child, productAreas);
            }   
        }

        private void CalcTotal(ProductAreaData productArea, ref int total)
        {
            total += productArea.Cases.Sum(c => c.Number);

            foreach (var child in productArea.Children)
            {
                this.CalcTotal(child, ref total);
            }
        }

        private void CalcTotalForCaseType(ProductAreaData productArea, CaseTypeData caseType, ref int total)
        {
            total += productArea.Cases.Where(c => c.CaseTypeId == caseType.Id).Sum(c => c.Number);

            foreach (var child in productArea.Children)
            {
                this.CalcTotalForCaseType(child, caseType, ref total);
            }
        }
    }
}