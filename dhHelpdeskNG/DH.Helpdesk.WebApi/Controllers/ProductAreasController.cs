using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Enums.Admin.Users;
using DH.Helpdesk.BusinessData.Enums.Users;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;
using DH.Helpdesk.WebApi.Infrastructure.Config.Authentication;
using DH.Helpdesk.WebApi.Infrastructure.Translate;

namespace DH.Helpdesk.WebApi.Controllers
{
    public class ProductAreasController : BaseApiController
    {
        private readonly IProductAreaService _productAreaService;
        private readonly IUserService _userSerivice;

        public ProductAreasController(IProductAreaService productAreaService, IUserService userSerivice)
        {
            _productAreaService = productAreaService;
            _userSerivice = userSerivice;
        }

        // GET api/<controller>
        public async Task<IEnumerable<ProductAreaOverview>> GetByCaseType(int cid, int? caseTypeId = null, int? includeId = null)
        {
            var user = await _userSerivice.GetUserOverviewAsync(UserId);//TODO: Use cached version?
            var productAreas = 
                _productAreaService.GetTopProductAreasForUserOnCase(cid, includeId, caseTypeId, user)
                    .OrderBy(p => p.Name)
                    .ToList();//TODO: Async

            //sort
            //productAreas = productAreas.OrderBy(p => Translation.GetMasterDataTranslation(p.Name)).ToList(); //TODO: translation for top and childs and order

            //var isTakeOnlyActive = true;
            //var userGroupId = User.Identity.GetGroupId();
            //var  userGroupDictionary = user.UserWorkingGroups.Where(it => it.UserRole == WorkingGroupUserPermission.ADMINSTRATOR).ToDictionary(it => it.WorkingGroup_Id, it => true);

            ////build tree with Ids
            //foreach (var pa in productAreas.Where(x => !isTakeOnlyActive || x.IsActive > 0))
            //{
            //    var childs = new List<ProductAreaOverview>();
            //    if (pa.SubProductAreas != null)
            //    {
            //        childs = pa.SubProductAreas.Where(p => !isTakeOnlyActive || p.IsActive > 0).ToList();

            //        if (userGroupId < (int) UserGroup.CustomerAdministrator)
            //        {
            //            childs =
            //                childs.Where(
            //                        it =>
            //                            it.WorkingGroups.Count == 0
            //                            || it.WorkingGroups.Any(wg => userGroupDictionary.ContainsKey(wg.Id))
            //                            || (includeId.HasValue && it.Id == includeId.Value))
            //                    .ToList();
            //        }
            //    }
            //}

            return productAreas;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        //private IEnumerable<int> GetSubProductAreasIds(ProductAreaOverview ctProductArea)
        //{
        //    var result = new List<int>();
        //    if (ctProductArea.SubProductAreas != null && ctProductArea.SubProductAreas.Any())
        //    {
        //        foreach (var subProductArea in ctProductArea.SubProductAreas)
        //        {
        //            if (subProductArea.IsActive == 1)
        //            {
        //                result.Add(subProductArea.Id);
        //                if (subProductArea.SubProductAreas != null && subProductArea.SubProductAreas.Any())
        //                {
        //                    var subAreaIds = GetSubProductAreasIds(subProductArea);
        //                    result.AddRange(subAreaIds);
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}
    }
}