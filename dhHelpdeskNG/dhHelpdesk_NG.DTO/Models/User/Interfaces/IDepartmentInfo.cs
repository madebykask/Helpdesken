namespace DH.Helpdesk.BusinessData.Models.User.Interfaces
{
    public interface IDepartmentInfo
    {
        string DepartmentId { get;  }
        string DepartmentName { get; }
        string SearchKey { get; }
        string CountryName { get; }
    }
}