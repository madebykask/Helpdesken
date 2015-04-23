namespace DH.Helpdesk.BusinessData.Models.Reports.Data.FinishingCauseCategoryCustomer
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class FinishingCauseCategoryCustomerCase
    {
        [IsId]
        public int DepartmentId { get; set; }

        public bool HasFinishingCause { get; set; }
    }
}