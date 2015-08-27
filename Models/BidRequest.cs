using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    public class BidRequest
    {
        /// <summary>
        /// Party to make bid for. 
        /// Specify this if the PartyId is for the represented Organization.
        /// No need to specify if the bid is made for current user party.
        /// </summary>
        public Guid? PartyId { get; set; }

        /// <summary>
        /// The bids to make.
        /// </summary>
        public List<Bid> Bids { get; set; }

        public BidRequest()
        {
        }

        public BidRequest(List<Bid> bids, Guid? partyId = null)
        {
            Bids = bids;
            PartyId = partyId;
        }

    }
}
