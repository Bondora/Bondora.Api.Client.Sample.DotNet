using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models.OAuth
{
    public class TokenResult
    {
        /// <summary>
        /// Identifies the type of token returned. At this time, this field will always have the value <c>Bearer</c>.
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// The token that must be sent to a API.
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// The lifetime in seconds of the access token. 
        /// For example, the value "3600" denotes that the access token will expire in one hour from the time the response was generated.
        /// 0 is used when the token dows not expire.
        /// </summary>
        public int expires_in { get; set; }

        /// <summary>
        /// Token Valid until in Unix Time Stamp (Number of seconds since Unix Epoch on January 1st, 1970 at UTC).
        /// </summary>
        public long valid_until { get; set; }

        public TokenResult()
        {
            token_type = "bearer";
        }
    }

    /// <summary>
    /// Result of the Token request
    /// </summary>
    public class AccessTokenResult : TokenResult
    {
        /// <summary>
        /// Refresh token that can be used to request new access token if the old token is expired.
        /// If the token does not support refreshing or the access token lifetime is not set, this will be <c>null</c>.
        /// <b>Returned only when request grant_type is authorization_code</b>
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// Scope(s) that the user has selected in the Authorization step.
        /// <b>Returned only when request grant_type is authorization_code</b>
        /// </summary>
        public string scope { get; set; }
    }

    public class RefreshTokenResult : TokenResult
    {

    }
}
