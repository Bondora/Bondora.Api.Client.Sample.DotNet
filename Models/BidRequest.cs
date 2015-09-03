using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    /// <summary>
    /// Bids to make for the user or user represented organization.
    /// </summary>
    public class BidRequest
    {
        /// <summary>
        /// Organization to make bid for. 
        /// Specify this if the Bid is made in behalf of the Organization.
        /// No need to specify if the bid is made for current user.
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// The bids to make.
        /// </summary>
        public List<Bid> Bids { get; set; }

        public BidRequest()
        {
        }

        public BidRequest(List<Bid> bids, Guid? organizationId = null)
        {
            Bids = bids;
            OrganizationId = organizationId;
        }

    }
}
