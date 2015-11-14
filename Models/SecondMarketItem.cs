using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bondora.Api.Client.Sample.DotNet.Models.Enums;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    /// <summary>
    /// SecondaryMarket list item's information
    /// </summary>
    public class SecondMarketItem
    {
        /// <summary>
        /// Item unique identifier
        /// </summary>
        public Guid Id { get; set; }

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
        /// Outstanding principal balance +/- discount or mark-up 
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Secondary market purchase fee paid to Bondora
        /// </summary>
        public decimal Fee { get; set; }

        /// <summary>
        /// Total amount paid for purchase
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Total amount still to be repaid by the borrower. This includes the principal balance, accrued interest and late charges as well as any future scheduled interest payments 
        /// </summary>
        public decimal OutstandingPayments { get; set; }

        /// <summary>
        /// Discount rate percent
        /// </summary>
        public decimal DesiredDiscountRate { get; set; }

        /// <summary>
        /// XIRR (extended internal rate of return) is a methodology to calculate the net return using the loan issued date and amount, 
        /// loan repayment dates and amounts and the principal balance according to the original repayment date. 
        /// All overdue principal payments are written off immediately. No provisions for future losses are made & only received (not accrued or scheduled) 
        /// interest payments are taken into account.
        /// </summary>
        public double? Xirr { get; set; }

        /// <summary>
        /// Date when item was published
        /// </summary>
        public DateTime? ListedOnDate { get; set; }
    }
}
