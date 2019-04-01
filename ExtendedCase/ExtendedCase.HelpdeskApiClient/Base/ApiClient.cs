using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExtendedCase.HelpdeskApiClient.Base
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;

        #region ctor()

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #endregion

        #region Get

        public async Task<HttpResponseMessage> Get(string requestUri)
        {
            var response = await _httpClient.GetAsync(requestUri);
            return response;
        }

        #endregion

        #region GetFormEncodedContent

        public async Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            using (var content = new FormUrlEncodedContent(values))
            {
                var query = await content.ReadAsStringAsync();
                var requestUriWithQuery = string.Concat(requestUri, "?", query);
                var response = await _httpClient.GetAsync(requestUriWithQuery);
                return response;
            }
        }

        #endregion

        #region PostFormEncodedContent

        public async Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values)
        {
            using (var content = new FormUrlEncodedContent(values))
            {
                var response = await _httpClient.PostAsync(requestUri, content);
                return response;
            }
        }

        #endregion

        #region PostJsonEncodedContent

        public async Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content)
        {
            var response = await _httpClient.PostAsJsonAsync(requestUri, content);
            return response;
        }

        #endregion

        #region PutJsonEncodedContent

        public async Task<HttpResponseMessage> PutJsonEncodedContent<T>(string requestUri, T content)
        {
            var response = await _httpClient.PutAsJsonAsync(requestUri, content);
            return response;
        }

        #endregion

    }

    public interface IApiClient
    {
        Task<HttpResponseMessage> Get(string requestUri);
        Task<HttpResponseMessage> GetFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostFormEncodedContent(string requestUri, params KeyValuePair<string, string>[] values);
        Task<HttpResponseMessage> PostJsonEncodedContent<T>(string requestUri, T content);

        Task<HttpResponseMessage> PutJsonEncodedContent<T>(string requestUri, T content);
    }
}