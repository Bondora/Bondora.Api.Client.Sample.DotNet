using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    public class RepresentedParty
    {
        /// <summary>
        /// Party Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Party (Organization) name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Date until the representation is active. 
        /// Null if no end date specified.
        /// </summary>
        public DateTime? ActiveToDate { get; set; }

        /// <summary>
        /// If the representation type is read only.
        /// </summary>
        public bool IsReadonly { get; set; }
    }
}
