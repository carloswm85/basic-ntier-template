using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Web.Controllers;

public class ErrorController : Controller
{
    private readonly ILogger<ErrorController> logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        this.logger = logger;
    }

    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode, string message, string errorId)
    {
        var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        ViewBag.OriginalPath = statusCodeData?.OriginalPath;
        ViewBag.OriginalQueryString = statusCodeData?.OriginalQueryString;

        switch (statusCode)
        {
            case 403:
                // EXPLANATION :
                //
                ViewBag.ErrorTitle = "Access forbidden";
                ViewBag.ErrorMessage = "You do not have permission to access this resource.";
                ViewBag.ErrorDetail = message;
                ViewBag.ErrorId = errorId;
                return View("Error");
            case 404:
                // EXPLANATION :
                //
                ViewBag.ErrorTitle = "Not found";
                ViewBag.ErrorMessage = "The requested resource was not found.";
                ViewBag.ErrorDetail = message;
                ViewBag.ErrorId = errorId;
                logger.LogWarning(
                    $"{statusCode} Error Ocurred. Path = {statusCodeData!.OriginalPath}"
                        + $" and QueryString = {statusCodeData.OriginalQueryString ?? "no-query-string"}"
                );
                break;
            case 405:
                // EXPLANATION :
                // A 405 status code, also known as "Method Not Allowed", is an HTTP response code that a server
                // sends when a client requests a method that the resource doesn't support.
                ViewBag.ErrorTitle = "Method not allowed";
                ViewBag.ErrorMessage = "The method used to access this resource is not allowed.";
                ViewBag.ErrorDetail = message;
                ViewBag.ErrorId = errorId;
                logger.LogWarning(
                    $"405 Error Ocurred. Path = {statusCodeData!.OriginalPath}"
                        + $" and QueryString = {statusCodeData.OriginalQueryString ?? "no-query-string"}"
                );
                break;
            case 500:
                // EXPLANATION :
                //
                ViewBag.ErrorTitle = "Server error";
                ViewBag.ErrorMessage = "An error occurred on the server.";
                ViewBag.ErrorDetail = message;
                ViewBag.ErrorId = errorId;
                return View("Error");
            default:
                // EXPLANATION :
                //
                ViewBag.ErrorTitle = "Error";
                ViewBag.ErrorMessage = "An error occurred while processing the request.";
                ViewBag.ErrorDetail = message;
                ViewBag.ErrorId = errorId;
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
            $"The path {exceptionDetails!.Path} threw an exception " + $" {exceptionDetails.Error}"
        );

        // Return a view showing a custom error page
        // TODO: Create the CustomError view?
        return View("CustomError");
    }
}
