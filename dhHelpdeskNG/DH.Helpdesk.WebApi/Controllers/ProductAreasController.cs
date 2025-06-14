﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.Services.Services.Cache;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/productareas")]
    public class ProductAreasController : BaseApiController
    {
        private readonly IProductAreaService _productAreaService;
        private readonly IUserService _userSerivice;
        private readonly ITranslateCacheService _translateCacheService;

        public ProductAreasController(IProductAreaService productAreaService, IUserService userSerivice, ITranslateCacheService translateCacheService)
        {
            _productAreaService = productAreaService;
            _userSerivice = userSerivice;
            _translateCacheService = translateCacheService;
        }

        /// <summary>
        /// Tree of product areas fiters by casetype
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="langId"></param>
        /// <param name="caseTypeId"></param>
        /// <param name="includeId"></param>
        /// <returns></returns>
        [Route("options")]
        public async Task<IEnumerable<ProductAreaOverview>> Get(int cid, int langId, int? caseTypeId = null, int? includeId = null)
        {
            var user = await _userSerivice.GetUserOverviewAsync(UserId);

            var productAreas =
                _productAreaService.GetProductAreasFiltered(cid, includeId, caseTypeId, user).ToList(); //TODO: async
          
            Translate(productAreas, langId, 0);

            return productAreas.OrderBy(p => p.Name);
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> GetProductArea(int id, int cid, int langId)
        {
            if (id > 0)
            {
                var productArea = await _productAreaService.GetProductAreaAsync(id);
                return Ok(new { id, workingGroupId = productArea.WorkingGroup_Id, 
                    priorityId = productArea.Priority_Id, 
                    parentId = productArea.Parent_ProductArea_Id });
            }

            return Ok();
        }

        private void Translate(List<ProductAreaOverview> products, int langId, int depth)
        {
            if (depth >= 20)
                throw new Exception("Iteration depth exceeded. Suspicion of infinte loop.");

            depth++;

            products.ForEach(p =>
            {
                p.Name = _translateCacheService.GetMasterDataTextTranslation(p.Name, langId);
                if (p.SubProductAreas != null && p.SubProductAreas.Any())
                {
                    Translate(p.SubProductAreas, langId, depth);
                }
            });
        }
    }
}