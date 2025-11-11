using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Web.MVC.Controllers.Examples
{
    public class ContosoUniversityController : Controller
    {
        private readonly ILogger<ContosoUniversityController> _logger;
        private readonly IContosoUniversityService _contosoUniversity;

        public ContosoUniversityController(
            IContosoUniversityService contosoUniversity,
            ILogger<ContosoUniversityController> logger
            )
        {
            _logger = logger;
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
        public async Task<IActionResult> StudentDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _contosoUniversity.GetStudentDetailsAsync(id.Value);


            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult StudentCreate()
        {
            var student = new Student();
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> StudentCreate(
            [Bind("EnrollmentDate,FirstMidName,LastName")]
            Student student
        )
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _contosoUniversity.SaveStudentAsync(student);
                    return RedirectToAction(nameof(Students));
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new student.");
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(student);
        }
    }
}