namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class FinishingCauseCategoryCustomerData
    {
        public FinishingCauseCategoryCustomerData(List<FinishingCauseCategoryCustomerRow> rows)
        {
            this.Rows = rows;
        }

        public List<FinishingCauseCategoryCustomerRow> Rows { get; private set; }

        public int GetNumberOfUsersSum()
        {
            return this.Rows.Select(r => r.NumberOfUsers).Sum();
        }

        public int GetNumberOfFinishedCasesSum()
        {
            return this.Rows.Select(r => r.NumberOfFinishedCases).Sum();
        }
    }
}