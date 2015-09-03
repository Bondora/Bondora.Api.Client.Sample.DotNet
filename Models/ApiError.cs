using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondora.Api.Client.Sample.DotNet.Models
{
    /// <summary>
    /// API Error object. Describes the error that occured.
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// Code of the error. For machine reading.
        /// </summary>
        [DefaultValue(0)]
        public int Code { get; set; }

        /// <summary>
        /// The error message for human reading.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Error details, if any. 
        /// For example the non valid Field's name or the Id of the failed object.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Creates new ApiError class with specified values
        /// </summary>
        /// <param name="code">Code of the message. For internal use</param>
        /// <param name="message">Error message</param>
        /// <param name="details">Details about the message</param>
        public ApiError(int code, string message, string details)
        {
            Code = code;
            Message = message;
            Details = details;
        }

        public ApiError() { }
    }
}
