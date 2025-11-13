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

        // GET: /contoso/students
        [HttpGet("students")]
        public async Task<IActionResult> StudentIndex()
        {
            var students = await _contosoService.GetStudentListAsync();
            return View(students);
        }

        // GET: /contoso/details/5
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> StudentDetails(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // GET: /contoso/create
        [AllowAnonymous]
        [HttpGet("create")]
        public IActionResult StudentCreate()
        {
            return View();
        }

        // POST: /contoso/create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // In this method: Use entity classes with model binding instead of view models.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentCreate([Bind("Id,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (!ModelState.IsValid)
                return View(student);

            try
            {
                await _contosoService.SaveStudentAsync(student);
                return RedirectToAction(nameof(StudentIndex));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new student.");
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
                return View(student);
            }
        }

        // GET: /contoso/edit/5
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> StudentEdit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // POST: /contoso/edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // POST: /contoso/edit/5
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentEdit(int id, [Bind("Id,LastName,FirstMidName,EnrollmentDate")] Student student)
        {
            if (id != student.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(student);

            try
            {
                await _contosoService.UpdateStudentAsync(student);
                return RedirectToAction(nameof(StudentIndex));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_contosoService.StudentExists(student.Id))
                    return NotFound();

                _logger.LogWarning(ex, "Concurrency conflict while updating student ID {StudentId}.", student.Id);
                ModelState.AddModelError("", "The record you attempted to edit was modified by another user.");
                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the student with ID {StudentId}.", student.Id);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                return View(student);
            }
        }


        // GET: /contoso/delete/5
        // Display student before deletion.
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> StudentDelete(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null) return NotFound();
            return View(student);
        }

        // POST: /contoso/delete/5
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StudentDeleteConfirmed(int id)
        {
            await _contosoService.DeleteStudentAsync(id);
            return RedirectToAction(nameof(StudentIndex));
        }

        // ADIITONAL METHODS CAN GO HERE

        /*
         *  | Method    | Scope                | Request Body        | Example Use Case                 |
         *  | --------- | -------------------- | ------------------- | -------------------------------- |
         *  | **PUT**   | Full replacement     | Full entity         | Replace an entire student record |
         *  | **PATCH** | Partial modification | JSON Patch document | Update one or more fields only   |
         */

        // PUT: /contoso/student/5
        [HttpPut("student/{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.Id)
                return BadRequest("Student ID mismatch.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existingStudent = await _contosoService.GetStudentAsync(id);
                if (existingStudent == null)
                    return NotFound();

                // Replace all properties (full update)
                existingStudent.FirstMidName = student.FirstMidName;
                existingStudent.LastName = student.LastName;
                existingStudent.EnrollmentDate = student.EnrollmentDate;

                await _contosoService.SaveStudentAsync(existingStudent);
                return NoContent(); // 204 - successful update
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating student.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data.");
            }
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
