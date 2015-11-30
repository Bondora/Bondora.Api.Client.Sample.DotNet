using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    public class Auction
    {
        public Guid LoanId { get; set; }
        public Guid AuctionId { get; set; }
        public int LoanNumber { get; set; }
        public string UserName { get; set; }
        public int NewCreditCustomer { get; set; }
        public DateTime LoanApplicationStartedDate { get; set; }
        public int ApplicationSignedHour { get; set; }
        public int ApplicationSignedWeekday { get; set; }
        public int VerificationType { get; set; }
        public int LanguageCode { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public string Country { get; set; }
        public int CreditScore { get; set; }
        public string CreditGroup { get; set; }
        public decimal AppliedAmount { get; set; }
        public decimal Interest { get; set; }
        public int LoanDuration { get; set; }
        public string County { get; set; }
        public string City { get; set; }
        public int UseOfLoan { get; set; }
        public int Education { get; set; }
        public int MaritalStatus { get; set; }
        public int NrOfDependants { get; set; }
        public int EmploymentStatus { get; set; }
        public string EmploymentDurationCurrentEmployer { get; set; }
        public string WorkExperience { get; set; }
        public int OccupationArea { get; set; }
        public int HomeOwnershipType { get; set; }
        public decimal IncomeFromPrincipalEmployer { get; set; }
        public decimal IncomeFromPension { get; set; }
        public decimal IncomeFromFamilyAllowance { get; set; }
        public decimal IncomeFromSocialWelfare { get; set; }
        public decimal IncomeFromLeavePay { get; set; }
        public decimal IncomeFromChildSupport { get; set; }
        public decimal IncomeOther { get; set; }
        public decimal IncomeTotal { get; set; }
        public int MonthlyPaymentDay { get; set; }
        public DateTime ScoringDate { get; set; }
        public int ModelVersion { get; set; }
        public double ExpectedLoss { get; set; }
        public string Rating { get; set; }
        public double CureRate { get; set; }
        public double EADRate { get; set; }
        public double LossGivenDefault { get; set; }
        public double MaturityFactor { get; set; }
        public double ProbabilityOfBad { get; set; }
        public double ProbabilityOfDefault { get; set; }
        public double ExpectedReturnAlpha { get; set; }
        public double InterestRateAlpha { get; set; }
    }
}
