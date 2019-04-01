using System.Collections.Generic;
using System.Threading;
using System.Web.Http;
using ExtendedCase.Logic.Services;
using ExtendedCase.Models;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.WebApi.Controllers
{
    [RoutePrefix("api/OptionDataSourceData")]
    public class OptionDataSourceDataController : ExtendedCaseApiControllerBase
    {
        private readonly IOptionDataSourceService _optionDataSourceService;

        public OptionDataSourceDataController(IOptionDataSourceService optionDataSourceService)
        {
            _optionDataSourceService = optionDataSourceService;
        }

        // POST: api/DataSourceOptions
        [HttpPost]
        public IList<DataSourceOptionModel> GetOptionsByQuery([FromBody]JObject query)
        {
            return _optionDataSourceService.GetOptions(query);
        }
    }
}