using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models.Enums
{
    public enum ApiBidFailureReason
    {
        AvailableAmountLowerThanMinInvestmentLimit = 1,
        BiddingOnOwnAuction = 2,
        BiddingOnInactiveDuplicate = 3,
        BiddingAmountTooSmall = 4,
        NotEnoughBalance = 5,
        AuctionIsClosed = 6,
        AuctionStatusNotOpen = 7,
        NoRiskScoreForAuction = 8,
        AuctionAlreadyFullyBidded = 9,
        AuctionNotFound = 10,
        NotEnoughLoanAmountForBiddingAmount = 11,
        ApiUsageNotAllowed = 12,
        Unknown = 13
    }
}
