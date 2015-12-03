using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models.OAuth
{
    /// <summary>
    /// OAuth Access Token request parameters
    /// </summary>
    public class AccessTokenRequest
    {
        /// <summary>
        /// Grant type for getting the access token.
        /// <c>authorization_code</c> for getting access token for authorization code
        /// <c>refresh_token</c> for getting access token for refresh token
        /// </summary>
        public string grant_type { get; set; }

        /// <summary>
        /// Identifies the client that is making the request. 
        /// The value passed in this parameter must exactly match the value in the registred Application.
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// The client secret that is created for the Application.
        /// </summary>
        public string client_secret { get; set; }

        

        /// <summary>
        /// If set, must be the same as the provided in the OAuth Authorize step.
        /// </summary>
        public string redirect_uri { get; set; }
    }

    public class AccessTokenCodeRequest : AccessTokenRequest
    {
        /// <summary>
        /// Code that was returned from the OAuth Authorize step.
        /// Used when requesting token for exchange of authorization code.
        /// Set the <c>grant_type</c> to <c>authorization_code</c>.
        /// </summary>
        public string code { get; set; }

        public AccessTokenCodeRequest()
        {
            grant_type = "authorization_code";
        }
    }

    public class AccessTokenRefreshTokenRequest : AccessTokenRequest
    {
        /// <summary>
        /// Refresh token that was returned in token response for exchange of code.
        /// Used when requesting token for exchange of refresh token code.
        /// Set the <c>grant_type</c> to <c>refresh_token</c>.
        /// </summary>
        public string refresh_token { get; set; }

        public AccessTokenRefreshTokenRequest()
        {
            grant_type = "refresh_token";
        }
    }
}
