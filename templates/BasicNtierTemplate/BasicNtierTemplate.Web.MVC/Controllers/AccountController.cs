using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers
{
    public class AccountController(IHttpClientFactory httpClientFactory) : Controller
    {
        private readonly HttpClient _client = httpClientFactory.CreateClient("ApiClient");

        public IActionResult Index()
        {
            return View();
        }
    }
}