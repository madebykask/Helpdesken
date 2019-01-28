using System.Web.Http;
using ExtendedCase.Logic.Services;

namespace ExtendedCase.WebApi.Controllers
{
    [RoutePrefix("api/OptionDataSources")]
    public class OptionDataSourcesController : ExtendedCaseApiControllerBase
    {
        private readonly IOptionDataSourceService _optionDataSourceService;

        public OptionDataSourcesController(IOptionDataSourceService optionDataSourceService)
        {
            _optionDataSourceService = optionDataSourceService;
        }

        // GET: api/OptionDataSources/5
        [Route("{id:int}")]
        public string Get(int id)
        {
            return "111";
        }

        // GET: api/OptionDataSources/sdfsdf
        [HttpGet]
        [Route("{dataSourceId}")]
        public string ByDataSourceId(string dataSourceId)
        {
            return "222";
        }
    }
}