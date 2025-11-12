using System.Net;
using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Web.MVC.Controllers.Examples
{
    [Route("contoso")]
    public class ContosoUniversityController : Controller
    {
        private readonly ILogger<ContosoUniversityController> _logger;
        private readonly IContosoUniversityService _contosoService;

        public ContosoUniversityController(
            IContosoUniversityService contosoUniversity,
            ILogger<ContosoUniversityController> logger
            )
        {
            _logger = logger;
            _contosoService = contosoUniversity;
        }

        #region STUDENTS

        // GET: /Contoso/Students
        [HttpGet("students")]
        public async Task<IActionResult> StudentIndex()
        {
            var students = await _contosoService.GetStudentListAsync();
            return View(students);
        }

        // GET: /Contoso/Details/5
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> StudentDetails(int? id)
        {
            if (id == null) return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null) return NotFound();

            return View(student);
        }

        // GET: /Contoso/StudentCreate
        [AllowAnonymous]
        [HttpGet("create")]
        public IActionResult StudentCreate()
        {
            return View();
        }

        // POST: /Contoso/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // In this method: Use entity classes with model binding instead of view models.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentCreate([Bind("Id,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
                try
                {
                    if (ModelState.IsValid)
                    {
                        await _contosoService.SaveStudentAsync(student);
                        return RedirectToAction(nameof(StudentIndex));
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

        // GET: /Contoso/Edit/5
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> StudentEdit(int? id)
        {
            if (id == null) return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null) return NotFound();

            return View(student);
        }

        // POST: /Contoso/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEdit(int id, [Bind("Id,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (id != student.Id) return NotFound();
            if (!_contosoService.StudentExists(student.Id)) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _contosoService.UpdateStudent(student);
                    return RedirectToAction(nameof(StudentIndex));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while editing the student with ID {StudentId}.", student.Id);
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                    throw;
                }
            }
            return View(student);
        }

        // GET: /Contoso/Delete/5
        // Display student before deletion.
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> StudentDelete(int? id)
        {
            if (id == null) return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /Contoso/Delete/5
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentDeleteConfirmed(int id)
        {
            await _contosoService.DeleteStudent(id);
            return RedirectToAction(nameof(StudentIndex));
        }

        #endregion

        #region COURSES

        // TODO

        #endregion

        #region INSTRUCTORS

        // TODO

        #endregion

        #region DEPARTMENTS

        // TODO

        #endregion
    }
}
