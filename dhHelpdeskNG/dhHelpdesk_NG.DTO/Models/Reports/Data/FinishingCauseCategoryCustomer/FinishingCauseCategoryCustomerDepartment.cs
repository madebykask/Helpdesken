namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FinishingCauseCategoryCustomerDepartment
    {
        [IsId]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    
        public int NumberOfUsers { get; set; }
    }
}