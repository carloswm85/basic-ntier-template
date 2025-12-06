using BasicNtierTemplate.Data.Model;
using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Models;
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

        // GET: /Student/List
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
                var students = await _contosoService.GetStudentsPaginatedListAsync(
                    currentFilter, pageIndex, pageSize, searchString, sortOrder);

                var studentsPaginatedViewModel = new PaginatedListViewModel<StudentDto>(
                    paginatedList: students,
                    currentFilter: searchString,
                    currentSort: sortOrder,
                    sortColumnOne: string.IsNullOrEmpty(sortOrder)
                        ? CurrentSort.LastNameDesc : CurrentSort.LastNameAsc,
                    sortColumnTwo: sortOrder == CurrentSort.DateAsc
                        ? CurrentSort.DateDesc : CurrentSort.DateAsc,
                    pageSize: pageSize
                );

                return View(studentsPaginatedViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the student list.");
                throw;
            }

        }

        // GET: /Student/Details/5
        [HttpGet("Details/{id:int}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var student = await _contosoService.GetStudentAsync(id.Value);

            if (student == null)
                return NotFound();

            return View(student);
        }

        // GET: /Student/Create
        [AllowAnonymous]
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // In this method: Use entity classes with model binding instead of view models.
        [HttpPost("Create")]
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

        // GET: /Student/Edit/5
        [HttpGet("Edit/{id:int}")]
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
        // POST: /Student/Edit/5
        [HttpPost("Edit/{id:int}")]
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

                _logger.LogWarning(ex, "Concurrency conflict while updating student ID {0}.", studentDto.Id);
                ModelState.AddModelError("", "The record you attempted to edit was modified by another user.");
                return View(studentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while editing the student with ID {0}.", studentDto.Id);
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, contact your system administrator.");
                return View(studentDto);
            }
        }

        // GET: /Student/Delete/5
        // Display student before deletion.
        [HttpGet("Delete/{id:int}")]
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

        // POST: /Student/Delete/5
        [HttpPost("Delete/{id:int}")]
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
                _logger.LogError(ex, "An error occurred while deleting the student with ID {0}.", id);
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
        // PUT: /Student/Student/5
        [HttpPut("Student/{id}")]
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

        [HttpGet("Statistics")]
        public async Task<IActionResult> About()
        {
            var studentData = await _contosoService.GetEnrollmentDateDataAsync();
            return View(studentData);
        }

        [HttpGet("AdditionalInstructions")]
        public IActionResult ContosoUniversityInstructions()
        {
            var instructions = @"
                == INSTRUCTIONS FOR THE CONTOSO UNIVERSITY CONTENT ==

                Link to tutorial: https://learn.microsoft.com/en-us/aspnet/core/data/ef-mvc/?view=aspnetcore-8.0

                - The tutorial is not completed in this example content on purpose.
                - Sections COMPLETED are the following:
                    1. Get started with EF Core in an ASP.NET MVC web app
                    2. Implement CRUD Functionality
                    3. Add sorting, filtering, and paging
                    4. Apply migrations to the Contoso University sample
                    5. Create a complex data model
                - The following sections are NOT COMPLETED, the developer should complete them as an exercise:
                    6. Read related data
                    7. Update related data
                    8. Handle concurrency
                    9. Implement inheritance
                    10. Learn about advanced scenarios
                    
                == SUGGESTED STEPS ==
                
                - General:
                    • From point (5) all required models were included (in the code base and the database)
                    • Review the model relationships, fix if necessary
                    • Implement complete CRUD for all entities, only `Student` entity was completed
                    • Instructions on how to do that are on points (6) to (10) from tutorial
                - What you can do and where (these are just general guidelines, not all steps to take):
                    A. Data Layer:
                        • Separate database annotations (used for EF) from view annotations (used in MVC)
                    B. Repository Layer:
                        • Include missing repositories
                    C. Service Layer:
                        • Generate missing DTOs and mapping profiles
                        • Add interfaces and services, you can use the StudentService as example
                    D. API Layer:
                        • Generate missing endpoints
                    E. MVC Layer:
                        • Generate controllers, view models and views for full CRUD (and listing)
                            → Hint: VS (IDE) scaffolding engine can help to quicken this part of the development
                        • View annotations from Data Layer should be moved here to the view models
                        • For the sake of briefty, student view works directly with DTOs, you should change the code to work with view models
                
                == ADDITIONAL SUGGESTIONS ==
                
                - You can connect the API directly to MVC, instead of going directly from the Service layer to MVC
                - Implement all the endpoints from the API in the Angular layer, or a mobile project
                ";
            return Ok(instructions);
        }
    }
}
