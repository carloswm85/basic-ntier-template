using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace NLayerTemplate.Web.MVC.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            ViewBag.OriginalPath = statusCodeData?.OriginalPath;
            ViewBag.OriginalQueryString = statusCodeData?.OriginalQueryString;

            switch (statusCode)
            {
                case 403:
                    ViewBag.ErrorMessage = "You don't have permission to access this resource.";
                    ViewBag.ErrorTitle = "Forbidden";
                    return View("Error");
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    logger.LogWarning(
                        $"{statusCode} Error Ocurred. Path = {statusCodeData!.OriginalPath}"
                            + $" and QueryString = {statusCodeData.OriginalQueryString ?? "no-query-string"}"
                    );
                    break;
                case 405:
                    // A 405 status code, also known as "Method Not Allowed", is an HTTP response code that a server
                    // sends when a client requests a method that the resource doesn't support.
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    logger.LogWarning(
                        $"405 Error Ocurred. Path = {statusCodeData!.OriginalPath}"
                            + $" and QueryString = {statusCodeData.OriginalQueryString ?? "no-query-string"}"
                    );
                    break;
                case 500:
                    ViewBag.ErrorMessage = "An internal server error occurred.";
                    ViewBag.ErrorTitle = "Server Error";
                    return View("Error");
                default:
                    ViewBag.ErrorMessage = "An error occurred processing your request.";
                    ViewBag.ErrorTitle = "Error";
                    return View("Error");
            }

            return View("NotFound");
        }

        // TODO
        [Route("Error")]
        [AllowAnonymous]
        public ActionResult Error()
        {
            // Retrieve exception details from the HttpContext features.
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            // Log the path that caused the exception and the exception message.
            logger.LogError(
                $"The path {exceptionDetails!.Path} threw an exception "
                    + $" {exceptionDetails.Error}"
            );

            // Return a view showing a custom error page
            // TODO: Create the CustomError view?
            return View("CustomError");
        }
    }
}
