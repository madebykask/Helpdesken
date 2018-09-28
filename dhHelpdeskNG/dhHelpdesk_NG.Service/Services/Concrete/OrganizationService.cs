namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    
    using DH.Helpdesk.Dal.Repositories;
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

        List<OU> GetCustomerOUs(int customerId);
    }

    public class OrganizationService : IOrganizationService
    {
        private readonly IRegionRepository regionRepository;

        private readonly IDepartmentRepository departmentRepository;

        private readonly IDomainRepository domainRepository;

        private readonly IOrganizationUnitRepository organizationUnitRepository;

        public OrganizationService(
            IRegionRepository regionRepository,
            IDepartmentRepository departmentRepository,
            IDomainRepository domainRepository,
            IOrganizationUnitRepository organizationUnitRepository)
        {
            this.regionRepository = regionRepository;
            this.departmentRepository = departmentRepository;
            this.domainRepository = domainRepository;
            this.organizationUnitRepository = organizationUnitRepository;
        }

        public List<ItemOverview> GetRegions(int customerId)
        {
            return this.regionRepository.FindByCustomerId(customerId);
        }

        public List<ItemOverview> GetDepartments(int customerId)
        {
            return this.departmentRepository.FindActiveOverviews(customerId);
        }

        public List<ItemOverview> GetDepartments(int customerId, int? regionId)
        {
            return !regionId.HasValue
                       ? this.GetDepartments(customerId)
                       : this.departmentRepository.FindActiveByCustomerIdAndRegionId(customerId, regionId.Value);
        }

        public List<ItemOverview> GetDomains(int customerId)
        {
            return this.domainRepository.FindByCustomerId(customerId);
        }

        public List<ItemOverview> GetOrganizationUnits()
        {
            return this.organizationUnitRepository.FindActiveAndShowable();
        }

        public List<ItemOverview> GetOrganizationUnits(int? departmentId)
        {
            var emptyList = new List<ItemOverview>();
            return departmentId == null ?
                emptyList :
                this.organizationUnitRepository.FindActive(departmentId);
        }

        public List<OU> GetOUs(int? departmentId)
        {
            var emptyList = new List<OU>();
            return !departmentId.HasValue ?
                emptyList :
                this.organizationUnitRepository.GetOUs(departmentId).OrderBy(o=> o.Name).ToList();
        }

        public List<OU> GetCustomerOUs(int customerId)
        {            
            return this.organizationUnitRepository.GetCustomerOUs(customerId).OrderBy(o => o.Name).ToList();
        }
    }
}