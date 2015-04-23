namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCustomer
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Department;

    public sealed class FinishingCauseCustomerData
    {
        public FinishingCauseCustomerData(
            List<FinishingCauseRow> rows, 
            List<DepartmentOverview> departments)
        {
            this.Departments = departments;
            this.Rows = rows;
        }

        public List<FinishingCauseRow> Rows { get; private set; } 

        public List<DepartmentOverview> Departments { get; private set; }
    }
}