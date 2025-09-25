using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.API.Filters;
using University.Core.DTOs;
using University.Core.Forms;
using University.Core.Services;

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ApiResponse GetById(int id)
        {
            var dto = _studentService.GetStudentById(id);
            return new ApiResponse(dto);
        }
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ApiResponse GetAll()
        {
            var dto = _studentService.GetAllStudents();
            return new ApiResponse(dto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ApiResponse Add([FromBody] CreateStudentForm form )
        {
            _studentService.CreateStudent(form);
            return new ApiResponse(HttpStatusCode.Created);
        }

        [HttpPut("{id}")]
        public ApiResponse Update(int id, [FromBody] UpdateStudentForm form)
        {
            _studentService.UpdateStudent(id, form);
            return new ApiResponse(HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public ApiResponse Delete(int id)
        {
            _studentService.DeleteStudent(id);
            return new ApiResponse(HttpStatusCode.NoContent);
        }

    }
}
