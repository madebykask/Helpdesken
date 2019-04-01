using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Common.Logging;
using ExtendedCase.HelpdeskApiClient.Models;
using ExtendedCase.HelpdeskApiClient.Responses;

namespace ExtendedCase.HelpdeskApiClient.Base
{
    public abstract class ClientBase
    {
        protected readonly IApiClient ApiClient;
        protected ILogger Logger { get; }

        #region ctor()

        protected ClientBase(IApiClient apiClient, ILogger logger)
        {
            Logger = logger;
            ApiClient = apiClient;
        }

        #endregion

        #region Protected Methods

        protected async Task<string> GetRaw(string uri)
        {
            using (var apiResponse = await ApiClient.Get(uri))
            {
                var raw = await ReadContentAsString(apiResponse);
                return raw;
            }
        }

        protected async Task<TResponse> GetContent<TResponse, TContentResponse>(string uri)
          where TResponse : ApiResponse<TContentResponse>, new()
        {
            using (var apiResponse = await ApiClient.Get(uri))
            {
                return await ProcessJsonResponse<TResponse, TContentResponse>(apiResponse);
            }
        }

        // get form url encoded
        protected async Task<TResponse> GetFormEncodedContent<TRequest, TResponse, TContentResponse>(string uri, TRequest request)
            where TRequest : class
            where TResponse : ApiResponse<TContentResponse>, new()
        {
            var requestParameters = ConvertToKeyValuePairs(request);
            using (var apiResponse = await ApiClient.GetFormEncodedContent(uri, requestParameters))
            {
                return await ProcessJsonResponse<TResponse, TContentResponse>(apiResponse);
            }
        }

        // post form
        protected async Task<TResponse> PostFormEncodedContent<TResponse, TContentResponse, TRequestData>(string uri, TRequestData requestBody)
            where TRequestData : class
            where TResponse : ApiResponse<TContentResponse>, new()
        {
            var requestParameters = ConvertToKeyValuePairs(requestBody);
            using (var apiResponse = await ApiClient.PostFormEncodedContent(uri, requestParameters))
            {
                return await ProcessJsonResponse<TResponse, TContentResponse>(apiResponse);
            }
        }
        
        // post json (result)
        protected async Task<TResponse> PostJsonWithResult<TResponse, TResult, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<TResult>, new()
        {
            using (var apiResponse = await ApiClient.PostJsonEncodedContent(url, model))
            {
                return await ProcessJsonResponse<TResponse, TResult>(apiResponse);
            }
        }

        // post json (no result)
        protected async Task<TResponse> PostJsonWithoutResult<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse, new()
        {
            using (var apiResponse = await ApiClient.PostJsonEncodedContent(url, model))
            {
                return await CreateResponse<TResponse>(apiResponse);
            }
        }

        // put json (no result)
        protected async Task<TResponse> PutJsonWithoutResult<TResponse, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse, new()
        {
            using (var apiResponse = await ApiClient.PutJsonEncodedContent(url, model))
            {
                return await CreateResponse<TResponse>(apiResponse);
            }
        }
        // put json (result)
        protected async Task<TResponse> PutJsonWithResult<TResponse, TResult, TModel>(string url, TModel model)
            where TModel : ApiModel
            where TResponse : ApiResponse<TResult>, new()
        {
            using (var apiResponse = await ApiClient.PutJsonEncodedContent(url, model))
            {
                return await ProcessJsonResponse<TResponse, TResult>(apiResponse);
            }
        }

        #endregion

        #region Private Methods

        private static async Task<TResponse> ProcessJsonResponse<TResponse, TDecode>(HttpResponseMessage apiResponse)
            where TResponse : ApiResponse<TDecode>, new()
        {
            var response = await CreateResponse<TResponse>(apiResponse, readResponse:false);

            if (response.StatusIsSuccessful)
            {
                //decode data
                response.Data = await apiResponse.Content.ReadAsAsync<TDecode>();
            }

            return response;
        }

        protected static async Task<TResponse> CreateResponse<TResponse>(HttpResponseMessage response, bool readResponse = true, bool decodeErrorState = true)
            where TResponse : ApiResponse, new()
        {
            var clientResponse = new TResponse
            {
                StatusIsSuccessful = response.IsSuccessStatusCode,
                ErrorState = response.IsSuccessStatusCode == false && decodeErrorState ? await ReadContentAs<ErrorStateResponse>(response) : null,
                ResponseCode = response.StatusCode,
                ResponseResult = readResponse ? await ReadContentAsString(response) : null
            };
            
            return clientResponse;
        }

        protected static async Task<string> ReadContentAsString(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        protected static async Task<TContentResponse> ReadContentAs<TContentResponse>(HttpResponseMessage response)
        {
          var content = await response.Content.ReadAsAsync<TContentResponse>();
          return content;
        }

        #endregion

        #region Protected Methods

        protected string BuildResourceUri(string baseUri, string query)
        {
            baseUri = baseUri?.TrimEnd('/');
            query = query?.TrimStart('/');
            
            return $"{baseUri}/{query}";
        }

        protected KeyValuePair<string, string>[] ConvertToKeyValuePairs<TEntity>(TEntity request)
            where TEntity : class
        {
            var properties = request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return properties.Select(prop => new KeyValuePair<string, string>(prop.Name, prop.GetValue(request, null)?.ToString())).ToArray();
        }

        #endregion
    }
}