using System.Configuration;

namespace Bondora.Api.Client.Sample.DotNet
{
    public static class ApiConfig
    {
        public static string ApiBaseUri
        {
            get { return ConfigurationManager.AppSettings.Get("ApiBaseUri"); }
        }

        public static string ClientId
        {
            get { return ConfigurationManager.AppSettings.Get("ClientId"); }
        }

        public static string ClientSecret
        {
            get { return ConfigurationManager.AppSettings.Get("ClientSecret"); }
        }

        public static string RedirectUri
        {
            get { return ConfigurationManager.AppSettings.Get("RedirectUri"); }
        }
        
        public static string OAuthAuthorizeUri
        {
            get { return ConfigurationManager.AppSettings.Get("OAuthAuthorizeUri"); }
        }

        public static string OAuthScopes
        {
            get { return ConfigurationManager.AppSettings.Get("OAuthScopes"); }
        }

    }
}
