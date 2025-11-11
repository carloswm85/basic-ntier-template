using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.Web.MVC.Controllers.Examples
{
    public class ContosoUniversityController : Controller
    {
        private readonly IContosoUniversityService _contosoUniversity;

        public ContosoUniversityController(IContosoUniversityService contosoUniversity)
        {
            _contosoUniversity = contosoUniversity;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Students()
        {
            var students = await _contosoUniversity.GetStudentsAsync();
            return View(students);
        }

        [AllowAnonymous]
        public IActionResult Example()
        {
            return Ok("This is an example route.");
        }
    }
}