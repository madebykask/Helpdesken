using System.Web;
using ExtendedCase.HelpdeskApiClient.Interfaces;

namespace ExtendedCase.WebApi.Infrastructure.TokenProviders
{
    public class HttpApiTokenProvider : IApiTokenProvider
    {
        private readonly HttpContextBase _httpContextBase;
        private const string ApiAuthTokenKey = "_authApiToken";
        private const string ApiRefreshTokenKey = "_refreshApiToken";

        #region ctor()

        public HttpApiTokenProvider()
        {
        }

        public HttpApiTokenProvider(HttpContextBase httpContextBase)
        {
            _httpContextBase = httpContextBase;
        }

        #endregion

        #region IApiTokenProvider implementation

        public string GetToken()
        {
            return GetValue(ApiAuthTokenKey);
        }

        public void SetToken(string token)
        {
            SetValue(ApiAuthTokenKey, token);
                
        }

        public string GetRefreshToken()
        {
            return GetValue(ApiRefreshTokenKey);
        }

        public void SetRefreshToken(string token)
        {
            SetValue(ApiRefreshTokenKey, token);
        }

        #endregion

        #region Private Methods 

        private string GetValue(string key)
        {
            var value = string.Empty;

            if (_httpContextBase.Items != null &&
                _httpContextBase.Items.Contains(key))
            {
                value = (_httpContextBase.Items[key] ?? string.Empty).ToString();                
            }

            return value;
        }

        private void SetValue(string key, string value)
        {
            if (_httpContextBase.Items != null)
            {
                _httpContextBase.Items[key] = value;
            }
        }

        #endregion
    }
}