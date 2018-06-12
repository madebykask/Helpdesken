namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IOrganizationUnitRepository : IRepository<OU>
    {
        List<ItemOverview> FindActiveAndShowable();

        List<ItemOverview> FindActive(int? departmentId);

        List<OU> GetOUs(int? departmentId);
        
        IEnumerable<OU> GetActiveAndShowable();

        List<OU> GetCustomerOUs(int customerId);

        List<OU> GetOUs(int customerId, int departmentId, bool? isActive = null);
    }
}
