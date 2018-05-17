using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DH.Helpdesk.Common.Extensions.Integer;

namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using System.Web.Helpers;
    using DH.Helpdesk.Common.Enums;       

    public class OrganizationController : BaseController
    {
        //
        // GET: /Organization/
        private readonly IRegionService _regionService;

        private readonly IDepartmentService _departmentService;

        private readonly IOUService _ouService;

        public OrganizationController(
            IMasterDataService masterDataService,
            IRegionService regionService, 
            IDepartmentService departmentService, 
            IOUService ouService)
            : base(masterDataService)
        {
            this._regionService = regionService;
            this._departmentService = departmentService;
            this._ouService = ouService;            
        }

        public JsonResult GetRegions(int customerId)
        {
            var regions = _regionService.GetRegionsOverview(customerId);            
            return Json(new { regions }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDepartments(int customerId)
        {
            var departments = _departmentService.GetDepartments(customerId, ActivationStatus.All)
                                            .Select(d => new
                                            {
                                                Id = d.Id,
                                                Name = d.DepartmentName,
                                                IsActive = d.IsActive.ToBool(),
                                                RegionId = d.Region_Id,
                                                DisabledForOrder = d.DisabledForOrder
                                            });

            return Json(new { departments }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOUs(int customerId)
        {
            var allOUs = _ouService.GetOUs(customerId);

            var firstLevel = allOUs.Where(o=> !o.Parent_OU_Id.HasValue)
                                   .Select(o => new 
                                        { 
                                            Id = o.Id, 
                                            Name = o.Name, 
                                            IsActive = o.IsActive.ToBool(), 
                                            DepartmentId = o.Department_Id    
                                        }).ToList();
            
            foreach (var ou in allOUs)
            {
                if (ou.SubOUs.Any())
                {
                    foreach (var subOU in ou.SubOUs)
                        firstLevel.Add(new
                        {
                            Id = subOU.Id,
                            Name = string.Format("{0} - {1}", ou.Name, subOU.Name),
                            IsActive = subOU.IsActive.ToBool(),
                            DepartmentId = subOU.Department_Id
                        });
                }
            }            

            var ous = firstLevel;            
            return Json(new { ous }, JsonRequestBehavior.AllowGet);
        }

    }
}
