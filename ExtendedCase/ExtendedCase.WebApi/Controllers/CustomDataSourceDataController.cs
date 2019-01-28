using System.Collections.Generic;
using System.Web.Http;
using ExtendedCase.Logic.Services;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.WebApi.Controllers
{
    [RoutePrefix("api/CustomDataSourceData")]
    public class CustomDataSourceDataController : ExtendedCaseApiControllerBase
    {
        private readonly ICustomDataSourceService _customDataSourceService;

        public CustomDataSourceDataController(ICustomDataSourceService customDataSourceService)
        {
            _customDataSourceService = customDataSourceService;
        }

        // POST: api/DataSourceOptions
        [HttpPost]
        public IList<IDictionary<string, string>> GetOptionsByQuery([FromBody]JObject query)
        {
            return _customDataSourceService.GetData(query);
        }
    }
}