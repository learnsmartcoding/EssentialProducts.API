using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnSmartCoding.EssentialProducts.API.Common.ActionResults
{
    /// <summary>
    /// Internal Server Error Object Class
    /// </summary>
    public class InternalServerErrorObjectResult : ObjectResult
    {
        /// <summary>
        /// Set the status Code to return 
        /// </summary>
        /// <param name="error"></param>
        /// <param name="statusCode"></param>
        public InternalServerErrorObjectResult(object error, int? statusCode = null)
            : base(error)
        {
            StatusCode = statusCode ?? StatusCodes.Status500InternalServerError;
        }
    }
}
