using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Services.Services.Concrete;
using DH.Helpdesk.Web.Infrastructure;
using DH.Helpdesk.Web.Models.Common;

namespace DH.Helpdesk.Web.Areas.Orders.Controllers.WebApi
{
    public class OrdersApiController : BaseApiController
    {
        private readonly IOrganizationService _organizationService;

        public OrdersApiController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public object SearchDepartmentsByRegionId([FromUri]NullableIdParams parameters)
        {
            var models = _organizationService.GetDepartments(
                SessionFacade.CurrentCustomer.Id,
                parameters.Id);

            return models;
        }
    }
}
