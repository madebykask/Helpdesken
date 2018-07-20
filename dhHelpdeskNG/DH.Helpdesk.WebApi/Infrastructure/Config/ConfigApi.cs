namespace DH.Helpdesk.WebApi.Infrastructure.Config
{
    public static class ConfigApi
    {
        public class Constants
        {
            public const string TokenEndPoint = "/token";
            public const string DefaultRouteName = "DefaultApi";
            public const string PublicClientId = "hd";

            public struct OwinContext
            {
                public const string ClientId = "as:client_id";
            }
        }
    }
}