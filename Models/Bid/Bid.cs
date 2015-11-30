using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    public class Bid
    {
        /// <summary>
        /// The Auction ID to bid to.
        /// </summary>
        public Guid AuctionId { get; set; }

        /// <summary>
        /// Amount to bid into Auction"/>
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Minimum bid to make into auction.
        /// If the available amount to invest into loan is less than specified minimum limit, 
        /// the investment into the auction's loan is not made.
        /// </summary>
        public decimal? MinAmount { get; set; }

        public Bid(Guid auctionId, decimal amount, decimal? minAmount)
        {
            AuctionId = auctionId;
            Amount = amount;
            MinAmount = minAmount;
        }

        public Bid(Guid auctionId, decimal amount)
        {
            AuctionId = auctionId;
            Amount = amount;
        }
    }
}
