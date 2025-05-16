using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public string? Details { get; set; }

        public ApiErrorResponse(int statusCode, string exceptionMessage, string details)
        {
            StatusCode = statusCode;
            ExceptionMessage = exceptionMessage;
            Details = details;
        }
    }
}