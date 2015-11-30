using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bondora.Api.Client.Sample.DotNet.Models.Enums;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    public class Investment
    {
        /// <summary>
        /// LoanPart unique identifier
        /// </summary>
        public Guid LoanPartId { get; set; }

        /// <summary>
        /// Investment amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Auction unique identifier
        /// </summary>
        public Guid AuctionId { get; set; }

        /// <summary>
        /// Auction name
        /// </summary>
        public string AuctionName { get; set; }

        /// <summary>
        /// Auction number
        /// </summary>
        public int AuctionNumber { get; set; }

        /// <summary>
        /// Auction bid number
        /// </summary>
        public int AuctionBidNumber { get; set; }

        /// <summary>
        /// Residency of the borrower
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// <para><c>1000</c> No previous payments problems</para>
        /// <para><c>900</c> Payments problems finished 24-36 months ago</para>
        /// <para><c>800</c> Payments problems finished 12-24 months ago</para>
        /// <para><c>700</c> Payments problems finished 6-12 months ago</para>
        /// <para><c>600</c> Payment problems finished &lt;6 months ago</para>
        /// <para><c>500</c> Active payment problems</para>
        /// </summary>
        public double? CreditScore { get; set; }

        /// <summary>
        /// Credit Group of the borrower
        /// </summary>
        public string CreditGroup { get; set; }

        /// <summary>
        /// Bondora Rating issued by the Rating model
        /// </summary>
        public string Rating { get; set; }

        /// <summary>
        /// Interest rate
        /// </summary>
        public decimal Interest { get; set; }

        /// <summary>
        /// Use of loan
        /// </summary>
        public AuctionPurpose UseOfLoan { get; set; }

        /// <summary>
        /// Income verification type
        /// </summary>
        public AuctionIncomeVerificationStatus? IncomeVerificationStatus { get; set; }

        /// <summary>
        /// Loan unique identifier
        /// </summary>
        public Guid LoanId { get; set; }

        /// <summary>
        /// Loan status code
        /// <para><c>2</c> Current</para>
        /// <para><c>100</c> Overdue</para>
        /// <para><c>5</c> 60+ days overdue</para>
        /// <para><c>4</c> Repaid</para>
        /// <para><c>8</c> Released</para>
        /// </summary>
        public int LoanStatusCode { get; set; }

        /// <summary>
        /// Borrower's username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Borrower's Gender
        /// </summary>
        public Sex Gender { get; set; }

        /// <summary>
        /// Borrower's date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Loan issued date
        /// </summary>
        public DateTime? SignedDate { get; set; }

        /// <summary>
        /// Last rescheduling date
        /// </summary>
        public DateTime? ReScheduledOn { get; set; }

        /// <summary>
        /// Debt occured on date
        /// </summary>
        public DateTime? DebtOccuredOn { get; set; }

        /// <summary>
        /// Debt occured on date
        /// </summary>
        public DateTime? DebtOccuredOnForSecondary { get; set; }

        /// <summary>
        /// Loan original lenght
        /// </summary>
        public int LoanDuration { get; set; }

        /// <summary>
        /// Next scheduled payment number
        /// </summary>
        public int NextPaymentNr { get; set; }

        /// <summary>
        /// Next scheduled payment date
        /// </summary>
        public DateTime? NextPaymentDate { get; set; }

        /// <summary>
        /// Next scheduled payment amount
        /// </summary>
        public decimal? NextPaymentSum { get; set; }

        /// <summary>
        /// Total number of scheduled payments
        /// </summary>
        public int NrOfScheduledPayments { get; set; }

        /// <summary>
        /// Total principal repaid amount
        /// </summary>
        public decimal PrincipalRepaid { get; set; }

        /// <summary>
        /// Total interest repaid amount
        /// </summary>
        public decimal InterestRepaid { get; set; }

        /// <summary>
        /// Total late charges paid amount
        /// </summary>
        public decimal LateAmountPaid { get; set; }

        /// <summary>
        /// Remaining principal amount 
        /// </summary>
        public decimal PrincipalRemaining { get; set; }

        /// <summary>
        /// Principal debt amount
        /// </summary>
        public decimal PrincipalLateAmount { get; set; }

        /// <summary>
        /// Interest debt amount
        /// </summary>
        public decimal InterestLateAmount { get; set; }

        /// <summary>
        /// Late charges debt amount
        /// </summary>
        public decimal PenaltyLateAmount { get; set; }

        /// <summary>
        /// Late amount total
        /// </summary>
        public decimal LateAmountTotal { get; set; }

        /// <summary>
        /// Date when investment was made or purchased from second market
        /// </summary>
        public DateTime PurchaseDate { get; set; }

        /// <summary>
        /// Investment selling date
        /// </summary>
        public DateTime? SoldDate { get; set; }

        /// <summary>
        /// Investment amount or secondary market purchase price
        /// </summary>
        public decimal PurchasePrice { get; set; }

        /// <summary>
        /// SecondMarket sale price
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// Date when item was listed on secondary market
        /// </summary>
        public DateTime? ListedInSecondMarketOn { get; set; }

        /// <summary>
        /// Latest debt management stage
        /// </summary>
        public LoanDebtManagementEventType? LatestDebtManagementStage { get; set; }

        /// <summary>
        /// Latest debt management date
        /// </summary>
        public DateTime? LatestDebtManagementDate { get; set; }

        public decimal NoteLoanTransfersMainAmount { get; set; }

        public decimal NoteLoanTransfersInterestAmount { get; set; }

        public decimal NoteLoanLateChargesPaid { get; set; }

        public decimal NoteLoanTransfersEarningsAmount { get; set; }

        public decimal NoteLoanTransfersTotalRepaimentsAmount { get; set; }
    }
}
