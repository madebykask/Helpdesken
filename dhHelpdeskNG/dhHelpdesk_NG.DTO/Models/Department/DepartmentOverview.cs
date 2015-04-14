namespace DH.Helpdesk.BusinessData.Models.Department
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class DepartmentOverview
    {
        [IsId]
        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }
    }
}