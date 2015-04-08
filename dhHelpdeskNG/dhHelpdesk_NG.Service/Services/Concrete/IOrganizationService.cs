namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Domain;

    public interface IOrganizationService
    {
        List<ItemOverview> GetRegions(int customerId);

        List<ItemOverview> GetDepartments(int customerId);

        List<ItemOverview> GetDepartments(int customerId, int? regionId);

        List<ItemOverview> GetDomains(int customerId);

        List<ItemOverview> GetOrganizationUnits();

        List<ItemOverview> GetOrganizationUnits(int? departmentId);

        List<OU> GetOUs(int? departmentId);
    }
}