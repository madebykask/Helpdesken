// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AjaxController.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the AjaxController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using DH.Helpdesk.Services.Services.Concrete;

namespace DH.Helpdesk.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Interfaces;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Services.Services;
    using DH.Helpdesk.Web.Infrastructure;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;

    /// <summary>
    /// The ajax controller.
    /// </summary>
    public class AjaxController : BaseController
    {
        /// <summary>
        /// The product area service.
        /// </summary>
        private readonly IProductAreaService productAreaService;
        private readonly ICausingPartService causingPartService;
        private readonly IDepartmentService departmentService;
        private readonly OrganizationJsonService _organizationJsonService;
        private readonly IWorkContext workContext;

        public AjaxController(
            IMasterDataService masterDataService,
            IProductAreaService productAreaService, 
            ICausingPartService causingPartService, 
            IDepartmentService departmentService, 
            OrganizationJsonService organizationJsonService,
            IWorkContext workContext)
            : base(masterDataService)
        {
            this.productAreaService = productAreaService;
            this.causingPartService = causingPartService;
            this.departmentService = departmentService;
            _organizationJsonService = organizationJsonService;
            this.workContext = workContext;
        }

        /// <summary>
        /// The product area summary.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [HttpGet]
        public JsonResult ProductArea(int customerId)
        {
            var list = new HierarchyList();
            var all = this.productAreaService.GetProductAreaOverviews(customerId);
            this.FillHierarchyList(null, list, all);
            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// The causing type.
        /// </summary>
        /// <param name="customerId">
        /// The customer Id.
        /// </param>
        /// <returns>
        /// The <see cref="JsonResult"/>.
        /// </returns>
        [HttpGet]
        public JsonResult CausingPart(int customerId)
        {
            var list = new HierarchyList();
            var all = this.causingPartService.GetActiveCausingParts(customerId);
            this.FillHierarchyList(null, list, all);

            if (list.Groups == null)            
                list = HierarchyList.GetEmpty();            

            return this.Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDepartments(int? regionId, int? administratorId, int departmentFilterFormat)
        {
            var departments = this.departmentService.GetUserDepartments(
                                    this.workContext.Customer.CustomerId,
                                    administratorId,
                                    regionId,
                                    departmentFilterFormat);
            return this.Json(departments, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUnits(int? departmentId, int customerId)
        {
            var departments = _organizationJsonService.GetActiveOUForDepartmentAsIdName(departmentId, customerId);;
            return this.Json(departments, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDepartmentUsers(int? departmentId, int? workingGroupId)
        {
            var users = this.departmentService.GetDepartmentUsers(
                                    this.workContext.Customer.CustomerId,
                                    departmentId,
                                    workingGroupId);

            return this.Json(users, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// The fill hierarchy list.
        /// </summary>
        /// <param name="level">
        /// The item.
        /// </param>
        /// <param name="list">
        /// The list.
        /// </param>
        /// <param name="all">
        /// The all.
        /// </param>
        private void FillHierarchyList(
            IEnumerable<IHierarchyItem> level,
            HierarchyList list, 
            IEnumerable<IHierarchyItem> all)
        {
            IHierarchyItem[] brothers = level != null ? 
                all.Where(i => level.Select(l => (int?)l.Id).ToArray().Contains(i.ParentId)).ToArray() :
                all.Where(i => !i.ParentId.HasValue)
                .ToArray();
            if (!brothers.Any())
            {
                return;
            }

            var group = new ListGroup();
            var itemsList = new List<ListItem>();
            itemsList.Add(new ListItem()
            {
                Name = string.Empty
            });
            foreach (var brother in brothers)
            {
                itemsList.Add(new ListItem()
                                  {
                                      Id = brother.Id,
                                      ParentId = brother.ParentId,
                                      Name = Translation.GetMasterDataTranslation(brother.Name),
                                      Description = brother.Description
                                  });
            }
            group.Items = itemsList.ToArray();
            var groupsList = new List<ListGroup>();
            if (list.Groups != null)
            {
                groupsList.AddRange(list.Groups);
            }
            groupsList.Add(group);
            list.Groups = groupsList.ToArray();
            this.FillHierarchyList(brothers, list, all);
        }
    }
}
