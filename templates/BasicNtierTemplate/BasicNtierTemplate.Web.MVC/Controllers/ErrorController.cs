using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers
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
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    logger.LogWarning($"{statusCode} Error Ocurred. Path = {statusCodeResult!.OriginalPath}" +
                        $" and QueryString = {statusCodeResult.OriginalQueryString ?? "no-query-string"}");
                    break;
                case 405:
                    // A 405 status code, also known as "Method Not Allowed", is an HTTP response code that a server
                    // sends when a client requests a method that the resource doesn't support.
                    ViewBag.ErrorMessage = "Sorry, the resource you requested could not be found.";
                    logger.LogWarning($"405 Error Ocurred. Path = {statusCodeResult!.OriginalPath}" +
                        $" and QueryString = {statusCodeResult.OriginalQueryString ?? "no-query-string"}");
                    break;
                default:
                    ViewBag.ErrorMessage = $"Status code has occured: {statusCode}";
                    break;
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
            logger.LogError($"The path {exceptionDetails!.Path} threw an exception " +
                $" {exceptionDetails.Error}");

            // Return a view showing a custom error page
            // TODO: Create the CustomError view?
            return View("CustomError");
        }

        [Route("Error/ProxyServerError")]
        [AllowAnonymous]
        public IActionResult ProxyServerError()
        {
            return View();
        }
    }
}
