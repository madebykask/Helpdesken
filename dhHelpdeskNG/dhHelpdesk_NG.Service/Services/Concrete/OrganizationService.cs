using DH.Helpdesk.BusinessData.Models;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using BusinessData.Models.Shared;
    
    using Dal.Repositories;
    using Domain;

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
        private readonly IRegionRepository _regionRepository;

        private readonly IDepartmentRepository _departmentRepository;

        private readonly IDomainRepository _domainRepository;

        private readonly IOrganizationUnitRepository _organizationUnitRepository;

        public OrganizationService(
            IRegionRepository regionRepository,
            IDepartmentRepository departmentRepository,
            IDomainRepository domainRepository,
            IOrganizationUnitRepository organizationUnitRepository)
        {
            _regionRepository = regionRepository;
            _departmentRepository = departmentRepository;
            _domainRepository = domainRepository;
            _organizationUnitRepository = organizationUnitRepository;
        }

        public List<ItemOverview> GetRegions(int customerId)
        {
            return _regionRepository.FindByCustomerId(customerId);
        }

        public List<ItemOverview> GetDepartments(int customerId)
        {
            return _departmentRepository.FindActiveOverviews(customerId);
        }

        public List<ItemOverview> GetDepartments(int customerId, int? regionId)
        {
            return !regionId.HasValue
                       ? GetDepartments(customerId)
                       : _departmentRepository.FindActiveByCustomerIdAndRegionId(customerId, regionId.Value);
        }

        public List<ItemOverview> GetDomains(int customerId)
        {
            return _domainRepository.FindByCustomerId(customerId);
        }

        public List<ItemOverview> GetOrganizationUnits()
        {
            return _organizationUnitRepository.FindActiveAndShowable();
        }

        public List<ItemOverview> GetOrganizationUnits(int? departmentId)
        {
            var emptyList = new List<ItemOverview>();
            return departmentId == null ?
                emptyList :
                _organizationUnitRepository.FindActive(departmentId);
        }

        public List<OU> GetOUs(int? departmentId)
        {
            var emptyList = new List<OU>();
            return !departmentId.HasValue ?
                emptyList :
                _organizationUnitRepository.GetOUs(departmentId).OrderBy(o=> o.Name).ToList();
        }

        public List<OU> GetCustomerOUs(int customerId)
        {            
            return _organizationUnitRepository.GetCustomerOUs(customerId).OrderBy(o => o.Name).ToList();
        }
    }
}