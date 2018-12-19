using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Services.Services;
using DH.Helpdesk.WebApi.Infrastructure;

namespace DH.Helpdesk.WebApi.Controllers
{
    [RoutePrefix("api/categories")]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// List of categories.
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        [Route("options")]
        public async Task<IEnumerable<CategoryOverview>> Get(int cid)
        {
            return await Task.FromResult(_categoryService.GetParentCategoriesWithChildren(cid, true));
        }

    }
}