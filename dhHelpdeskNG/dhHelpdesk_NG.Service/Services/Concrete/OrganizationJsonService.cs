namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Common.Types;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public class IdName
    {
        public int id;

        public string name;
    }

    public class OrganizationJsonService
    {
        private readonly IOUService ouService;
        private readonly IRegionService regionService;
        private readonly IDepartmentService departmentService;

        public OrganizationJsonService(
            IDepartmentService departmentService, 
            IRegionService regionService, 
            IOUService ouService)
        {
            this.departmentService = departmentService;
            this.regionService = regionService;
            this.ouService = ouService;
        }

        public IdName[] GetActiveOUForDepartmentAsIdName(int? departmentId, int customerId)
        {
            var prelist = departmentId.HasValue ? this.ouService.GetOUs(customerId).Where(e => e.IsActive == 1 && departmentId.GetValueOrDefault() == e.Department_Id) : null;

            var unionList = new Dictionary<int, string>();
            if (prelist != null)
            {
                foreach (var ou in prelist)
                {
                    unionList.Add(ou.Id, ou.Name);
                    foreach (var s in ou.SubOUs.Where(e => e.IsActive == 1))
                    {
                        unionList.Add(s.Id, ou.Name + " - " + s.Name);
                    }
                }
            }
            
            return unionList.Select(x => new IdName(){ id = x.Key, name = x.Value }).ToArray();
        }


        public IdName[] GetActiveDepartmentForRegion(int? regionId, int customerId, int departmentFilterFormat)
        {
            var dep = this.departmentService.GetDepartments(customerId)
                                             .Where(d => d.Region_Id == null || (d.Region != null && d.Region.IsActive != 0))
                                             .ToList();

            if (regionId.HasValue)
            {
                var curRegion = this.regionService.GetRegion(regionId.Value);
                if (curRegion.IsActive != 0)
                    dep = dep.Where(x => x.Region_Id == regionId).ToList();
            }
            
            return dep.Select(x => new IdName(){ id = x.Id, name = utils.ServiceUtils.departmentDescription(x, departmentFilterFormat) }).ToArray();
        }
    }
}