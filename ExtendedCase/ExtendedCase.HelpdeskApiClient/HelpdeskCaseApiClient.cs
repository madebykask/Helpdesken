using System.Threading.Tasks;
using Common.Logging;
using ExtendedCase.HelpdeskApiClient.Base;
using ExtendedCase.HelpdeskApiClient.Responses;
using Newtonsoft.Json.Linq;

namespace ExtendedCase.HelpdeskApiClient
{
    public class HelpdeskCaseApiClient : ClientBase, IHelpdeskCaseApiClient
    {
        private HelpdeskApiResources.CaseApiUrlsBuilder CaseApiUrls => HelpdeskApiResources.CaseApi;

        public HelpdeskCaseApiClient(IApiClient apiClient, ILogger logger) 
            : base(apiClient, logger)
        {
        }

        public async Task<string> GetCaseString(int caseId)
        {
            var actionUri = CaseApiUrls.BuildGetCaseUri(caseId);
            var content = await GetRaw(actionUri);
            return content;
        }

        public async Task<GetCaseResponse> GetCase(int caseId)
        {
            var actionUri = CaseApiUrls.BuildGetCaseUri(caseId);
            var content = await GetContent<GetCaseResponse, JObject>(actionUri);
            return content;
        }
    }

    public interface IHelpdeskCaseApiClient
    {
        Task<string> GetCaseString(int caseId);
        Task<GetCaseResponse> GetCase(int caseId);
    }
}