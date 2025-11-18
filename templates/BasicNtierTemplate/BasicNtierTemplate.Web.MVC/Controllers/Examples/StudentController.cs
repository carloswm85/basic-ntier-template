using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Services.Interfaces;
using BasicNtierTemplate.Web.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasicNtierTemplate.Web.MVC.Controllers.Examples
{
    [Route("Student")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IContosoUniversityService _contosoService;

        public StudentController(
            IContosoUniversityService contosoService,
            ILogger<StudentController> logger
            )
        {
            _logger = logger;
            _contosoService = contosoService;
        }

        // GET: /student/students
        [HttpGet("List")]
        public async Task<IActionResult> Index(
            string currentFilter = "",
            int pageIndex = 1,
            int pageSize = 10,
            string searchString = "",
            string sortOrder = ""
        )
        {
            try
            {
                var students = await _contosoService.GetStudentListAsync(
                    currentFilter, pageIndex, pageSize, searchString, sortOrder);

                var studentsPagedViewModel = new PaginatedListViewModel<StudentDto>(
                    paginatedList: students,
                    currentFilter: searchString,
                    currentSort: sortOrder,
                    sortParamOne: sortOrder == "Date" ? "date_desc" : "Date", // by date
                    sortParamTwo: string.IsNullOrEmpty(sortOrder) ? "name_desc" : "", // by last name
                    pageSize: pageSize
                    );

                return View(studentsPagedViewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the student list.");
                throw;
            }

        }

        // GET: /student/details/5
        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // GET: /student/create
        [AllowAnonymous]
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /student/create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // In this method: Use entity classes with model binding instead of view models.
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,LastName,FirstMidName,GovernmentId,EnrollmentDate")] StudentDto student)
        {
            if (!ModelState.IsValid)
                return View(student);

            try
            {
                await _contosoService.CreateStudentAsync(student);
                return RedirectToAction(nameof(Index));
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

        // GET: /student/edit/5
        [HttpGet("edit/{id:int}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // To protect from overposting attacks, enable the specific properties
        // you want to bind to.
        // POST: /student/edit/5
        [HttpPost("edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("Id,LastName,FirstMidName,GovernmentId,EnrollmentDate")] StudentDto studentDto)
        {
            if (Id != studentDto.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(studentDto);

            try
            {
                await _contosoService.UpdateStudentAsync(Id, studentDto);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!_contosoService.StudentExists(studentDto.Id))
                    return NotFound();

                _logger.LogWarning(ex, "Concurrency conflict while updating student ID {StudentId}.", studentDto.Id);
                ModelState.AddModelError("", "The record you attempted to edit was modified by another user.");
                return View(studentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the student with ID {StudentId}.", studentDto.Id);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                return View(studentDto);
            }
        }


        // GET: /student/delete/5
        // Display student before deletion.
        [HttpGet("delete/{id:int}")]
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
                return NotFound();

            var student = await _contosoService.GetStudentAsync(studentId: id.Value, asNoTracking: true);

            if (student == null) return NotFound();

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }

        // POST: /student/delete/5
        [HttpPost("delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _contosoService.GetStudentAsync(studentId: id, asNoTracking: true);

            if (student == null)
                return RedirectToAction(nameof(Index));

            try
            {
                await _contosoService.DeleteStudentAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the student with ID {StudentId}.", id);
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }

        }

        // ADIITONAL METHODS CAN GO HERE

        /*
         *  | Method    | Scope                | Request Body        | Example Use Case                 |
         *  | --------- | -------------------- | ------------------- | -------------------------------- |
         *  | **PUT**   | Full replacement     | Full entity         | Replace an entire student record |
         *  | **PATCH** | Partial modification | JSON Patch document | Update one or more fields only   |
         */

        // Example, unused
        // PUT: /student/student/5
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

                await _contosoService.CreateStudentAsync(existingStudent);
                return NoContent(); // 204 - successful update
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating student.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data.");
            }
        }

        public async Task<IActionResult> About()
        {
            var studentData = await _contosoService.GetEnrollmentDateDataAsync();
            return View(studentData);
        }
    }
}
