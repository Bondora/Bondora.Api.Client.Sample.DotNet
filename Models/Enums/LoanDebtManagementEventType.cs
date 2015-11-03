using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models.Enums
{
    public enum LoanDebtManagementEventType
    {
        Message = 1,
        SentToBailiff = 2,
        ExpeditedPaymentOrderIssued = 7,
        DebtFullyPaid = 9,
        SentToDebtRegistry = 14,
        DebtNotificationEmailSent = 15,
        LoanDefaulted = 16,

        /// <summary>
        /// Decision has been made and declared
        /// </summary>
        DecisionMadeAndDeclared = 20,

        /// <summary>
        /// Claim has been withdrawn
        /// </summary>
        //ClaimWithdrawn = 21,

        DeceasedCustomer = 22,

        /// <summary>
        /// Call has been made
        /// </summary>
        CallMade = 23,


        /// <summary>
        /// Notification SMS has been sent
        /// </summary>
        DebtNotificationSmsSent = 24
    }
}
