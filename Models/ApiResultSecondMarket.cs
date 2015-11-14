using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    public class ApiResultSecondMarket : ApiResult<IList<SecondMarketItem>>
    {
        /// <summary>
        /// Total number of SecondaryMarket items found
        /// </summary>
        public int TotalCount { get; set; }
    }
}
