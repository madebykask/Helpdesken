﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/regions")]
    public class RegionsController : BaseApiController
    {
        private readonly IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        /// <summary>
        /// List of regions.
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("options")]
        public async Task<List<ItemOverview>> Get(int cid)
        {
            var items = await _regionService.GetActiveRegionsAsync(cid);
            return items.OrderBy(r => r.Name).Select(r => new ItemOverview(r.Name, r.Id.ToString())).ToList();
        }
    }
}