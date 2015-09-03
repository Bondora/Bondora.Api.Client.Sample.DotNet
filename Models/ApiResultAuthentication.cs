using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    /// <summary>
    /// Authorization result
    /// </summary>
    public class ApiResultAuthentication : ApiResult<AuthTokenResult>
    {
    }

    /// <summary>
    /// Authorization token result
    /// </summary>
    public class AuthTokenResult
    {
        /// <summary>
        /// API token to use for requests
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Token validation time
        /// </summary>
        public DateTime ValidUntil { get; set; }

        /// <summary>
        /// List of User's represented Organization(s)
        /// </summary>
        public List<UserOrganization> UserOrganizations { get; set; }
    }
}
