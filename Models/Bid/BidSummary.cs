using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bondora.Api.Client.Sample.DotNet.Models.Enums;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    public class BidSummary
    {
        /// <summary>
        /// Bid unique identifier
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Id of auction bidded
        /// </summary>
        public virtual Guid AuctionId { get; set; }

        /// <summary>
        /// amount that is requested to bid
        /// </summary>
        public virtual decimal RequestedBidAmount { get; set; }

        /// <summary>
        /// amount that is bidded
        /// </summary>
        public virtual decimal? ActualBidAmount { get; set; }

        /// <summary>
        /// minimum amount that can be bidded for this auction
        /// </summary>
        public virtual decimal? RequestedBidMinimumLimit { get; set; }

        /// <summary>
        /// when bid is requested via API
        /// </summary>
        public virtual DateTime? BidRequestedDate { get; set; }

        /// <summary>
        /// when bid is placed by autobidder
        /// </summary>
        public virtual DateTime? BidProcessedDate { get; set; }

        /// <summary>
        /// Is request currently processed
        /// </summary>
        public virtual bool IsRequestBeingProcessed { get; set; }

        /// <summary>
        /// status of bid
        /// </summary>
        public virtual ApiBidStatusCode StatusCode { get; set; }

        /// <summary>
        /// why bid failed
        /// </summary>
        public virtual ApiBidFailureReason FailureReason { get; set; }

    }
}
