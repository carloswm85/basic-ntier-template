using BasicNtierTemplate.Service.Dtos;
using BasicNtierTemplate.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BasicNtierTemplate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContosoUniversityController : ControllerBase
    {
        private readonly IContosoUniversityService _contosoService;

        public ContosoUniversityController(IContosoUniversityService contosoService)
        {
            _contosoService = contosoService;
        }

        #region Student

        // ✔️ GET: api/students
        [HttpGet("students")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetStudents()
        {
            var students = await _contosoService.GetStudentListAsync();
            return Ok(students);
        }

        // ✔️ GET: api/students/5
        [HttpGet("students/{studentId:int})", Name = "GetStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetStudent(int studentId)
        {
            var studentDto = await _contosoService.GetStudentAsync(studentId);

            if (studentDto == null)
                return NotFound($"Student with id {studentId} was not found");

            return Ok(studentDto);
        }

        // ✔️ POST: api/students
        [HttpPost("students")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> CreateStudent([FromBody] StudentDto studentDto)
        {
            if (studentDto == null)
                return BadRequest(ModelState);

            if (_contosoService.StudentExists(studentDto.GovernmentId))
            {
                ModelState.AddModelError("CustomError", "Student already exists");
                return BadRequest(ModelState);
            }

            var studentIdResult = await _contosoService.CreateStudentAsync(studentDto);
            if (studentIdResult == 0)
            {
                ModelState.AddModelError("CustomError", $"Something went wrong when trying to save the student {studentDto.FirstMidName} {studentDto.LastName}");
                return StatusCode(500, ModelState);
            }

            var createdStudent = await _contosoService.GetStudentAsync(studentIdResult);
            return CreatedAtRoute("GetStudent", new { studentId = createdStudent!.Id }, createdStudent);
        }

        // ✔️ PUT: api/students
        [HttpPut("students/{studentId:int}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudent(int studentId, [FromBody] StudentDto studentDto)
        {
            if (studentDto == null)
                return BadRequest(ModelState);

            if (!_contosoService.StudentExists(studentDto.GovernmentId))
            {
                ModelState.AddModelError("CustomError", $"Student with government id {studentDto.GovernmentId} does not exist");
                return BadRequest(ModelState);
            }

            if (!await _contosoService.UpdateStudentAsync(studentId, studentDto))
            {
                ModelState.AddModelError("CustomError", $"Something went wrong when trying to save the student {studentDto.FirstMidName} {studentDto.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // ✔️ DELETE: api/students/5
        [HttpDelete("students/{studentId:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteStudent(int studentId)
        {
            if (!await _contosoService.DeleteStudentAsync(studentId))
            {
                ModelState.AddModelError("CustomError", $"Something went wrong when trying to eliminate the student with id {studentId}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        #endregion
    }
}
