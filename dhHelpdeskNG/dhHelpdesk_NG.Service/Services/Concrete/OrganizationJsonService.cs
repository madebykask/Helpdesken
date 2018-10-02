namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models;

    public class OrganizationJsonService
    {
        private readonly IOUService _ouService;
        private readonly IRegionService _regionService;
        private readonly IDepartmentService _departmentService;

        public OrganizationJsonService(
            IDepartmentService departmentService, 
            IRegionService regionService, 
            IOUService ouService)
        {
            _departmentService = departmentService;
            _regionService = regionService;
            _ouService = ouService;
        }

        public IdName[] GetActiveOUForDepartmentAsIdName(int? departmentId, int customerId)
        {
            var ous = _ouService.GetActiveOuForDepartment(departmentId, customerId);

            return ous.Select(ou =>
            {
                var name = ou.Name;
                if (ou.Parent_OU_Id.HasValue)
                {
                    var parentName = ous.FirstOrDefault(pou => pou.Id == ou.Parent_OU_Id.Value)?.Name;
                    if(!string.IsNullOrWhiteSpace(parentName)) name = $"{ou.Name} - {parentName}";
                }

                return new IdName {id = ou.Id, name = name};
            }).ToArray();
        }

        /// <summary>
        /// Returns Key-value array of departmens for specified region filtered by permissions (In Admin-User "Department" tab)
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="departmentFilterFormat"></param>
        /// <returns></returns>
        public IdName[] GetActiveDepartmentForUserByRegion(int? regionId, int userId, int customerId, int departmentFilterFormat)
        {
            var deps = _departmentService.GetActiveDepartmentForUserByRegion(regionId, userId, customerId);

            return deps.Select(x => new IdName() { id = x.Id, name = utils.ServiceUtils.departmentDescription(x, departmentFilterFormat) }).ToArray();
        }

        /// <summary>
        /// Returns Key-Value array of departments for specified region
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="customerId"></param>
        /// <param name="departmentFilterFormat"></param>
        /// <returns></returns>
        public IdName[] GetActiveDepartmentForRegion(int? regionId, int customerId, int departmentFilterFormat)//TODO: refactor move code to _departmentService
        {
            var dep = new List<Domain.Department>();
            if (regionId.HasValue)
                dep = this._departmentService.GetDepartmentsWithRegion(customerId)
                                             .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                                             .ToList();
            else
                dep = this._departmentService.GetDepartmentsWithRegion(customerId)
                                             .Where(d => d.IsActive != 0)
                                             .ToList();

            if (regionId.HasValue)
            {
                var curRegion = this._regionService.GetRegion(regionId.Value);
                if (curRegion.IsActive != 0)
                    dep = dep.Where(x => x.Region_Id == regionId).ToList();
            }
            
            return dep.Select(x => new IdName() { id = x.Id, name = utils.ServiceUtils.departmentDescription(x, departmentFilterFormat) }).ToArray();
        }
    }
}