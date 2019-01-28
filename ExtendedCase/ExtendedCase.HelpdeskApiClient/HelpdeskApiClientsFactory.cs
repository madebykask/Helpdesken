using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Common.Logging;
using ExtendedCase.HelpdeskApiClient.Base;
using ExtendedCase.HelpdeskApiClient.Configuration;
using ExtendedCase.HelpdeskApiClient.Handlers;
using ExtendedCase.HelpdeskApiClient.Interfaces;

namespace ExtendedCase.HelpdeskApiClient
{
    public class HelpdeskApiClientsFactory : IHelpdeskApiClientsFactory
    {
        private readonly IHelpdeskApiSettings _settings;
        private readonly IApiTokenProvider _tokenProvider;
        private readonly ILogger _logger;
        private readonly Lazy<IApiClient> _apiClient;
        private readonly Dictionary<Type, ClientBase> _clientInstances = new Dictionary<Type, ClientBase>();

        #region ctor()

        public HelpdeskApiClientsFactory(IHelpdeskApiSettings settings, IApiTokenProvider tokenProvider, ILogger logger)
        {
            _settings = settings;
            _tokenProvider = tokenProvider;
            _logger = logger;

            _apiClient = new Lazy<IApiClient>(CreateApiClient);
        }

        #endregion

        #region Api Clients Factory Methods

        public IHelpdeskCaseApiClient CreateCaseApiClient()
        {
            return CreateClient<HelpdeskCaseApiClient>();
        }

        #endregion

        #region Create Api Client

        private TApiClient CreateClient<TApiClient>()
            where TApiClient : ClientBase
        {
            var apiClientType = typeof(TApiClient);

            if (!_clientInstances.ContainsKey(apiClientType))
            {
                var clientInstance = (TApiClient)Activator.CreateInstance(typeof(TApiClient), _apiClient.Value, _logger);
                _clientInstances.Add(apiClientType, clientInstance);
            }

            return (TApiClient)_clientInstances[apiClientType];
        }

        private IApiClient CreateApiClient()
        {
            var instance = CreateHttpClient();
            var apiClient = new ApiClient(instance);
            return apiClient;
        }

        private HttpClient CreateHttpClient()
        {
            //create with delegating handlers
            var httpClient = 
                HttpClientFactory.Create(new HttpClientDiagnosticHandler(_logger),
                                         new HttpClientApiTokenInjectionHandler(_tokenProvider));

            httpClient.BaseAddress = new Uri(_settings.WebApiBaseUri);

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        #endregion
    }

    public interface IHelpdeskApiClientsFactory
    {
        IHelpdeskCaseApiClient CreateCaseApiClient();
    }
}