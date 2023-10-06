using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace CIS341_lab4.Pages
{
    public class ErrorModel : PageModel
    {
        public int origStatusCode { get; set; }

        public string errorMessage { get; set; }

        public string RequestedUrl { get; set; }

        public string? RequestId { get; set; }

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int statusCode)
        {
            origStatusCode = statusCode;
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            RequestedUrl = HttpContext.Request.Path;

            var statusCodeReExecuteFeature =
                HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            var userAgent = HttpContext.Request.Headers.UserAgent;

            //"Log relevant information about the error using the *conceptually appropriate* Log method in the Logger interface. 
            if (statusCodeReExecuteFeature != null)
            {
                if (statusCode == 404)
                {
                    //Include at least the client's user agent information, the requested URL, and the HTTP status code.
                    _logger.LogError("The content or page you are trying to access was not found.\n" +
                                     "Error code: {StatusCode}\n" +
                                     "URL/Route: {OriginalPath}\n" +
                                     "User agent: {UserAgent} \n ", statusCode, statusCodeReExecuteFeature.OriginalPath, userAgent);
                }
                else
                {
                    errorMessage = $"An error (status code {statusCode} has occurred. We apologize for the inconvenience.";
                }
            }

        }
    }
}