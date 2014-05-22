namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;

    public interface IOrganizationService
    {
        List<ItemOverview> GetRegions(int customerId);

        List<ItemOverview> GetDepartments(int customerId);

        List<ItemOverview> GetDepartments(int customerId, int? regionId);

        List<ItemOverview> GetDomains(int customerId);

        List<ItemOverview> GetOrganizationUnits();
    }
}