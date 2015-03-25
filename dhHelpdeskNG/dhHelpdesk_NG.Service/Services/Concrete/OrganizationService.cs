namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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

        public OU[] GetOrganizationUnitsBy(int customerId, int? regionId, int? departmentId)
        {
            var activeDepartments = this.departmentRepository.GetActiveDepartmentsBy(customerId, regionId);
            if (departmentId.HasValue)
            {
                activeDepartments = activeDepartments.Where(it => it.Id == departmentId.Value);
            }

            var data =
                this.organizationUnitRepository.GetActiveAndShowable()
                    .Join(
                        activeDepartments,
                        unit => unit.Department_Id,
                        department => department.Id,
                        (unit, department) => unit)
                        .OrderBy(it => it.Parent_OU_Id).ThenBy(it => it.Name).ToArray();

            return data;
        }
    }
}