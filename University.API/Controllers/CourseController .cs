using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using University.API.Filters;
using University.Core.DTOs;
using University.Core.Forms.CourseForms;
using University.Core.Services;

namespace University.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _CourseService;
        public CourseController(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        //============================== ENDOINTS FOR COURSES ============================//

        // Get Course by ID
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CourseDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> GetById(int id)
        {
            var dto = await _CourseService.GetCourseById(id);
            return new ApiResponse(dto);
        }

        // Get All Courses
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CourseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ApiResponse> GetAll()
        {
            var dto = await _CourseService.GetAllCourses();
            return new ApiResponse(dto);
        }

        // Add new Course
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ApiResponse> Add([FromBody] CreateCourseForm form )
        {
            await _CourseService.CreateCourse(form);
            return new ApiResponse(HttpStatusCode.Created);
        }

        // Update Course by ID
        [HttpPut("{id}")]
        public async Task<ApiResponse> Update(int id, [FromBody] UpdateCourseForm form)
        {
            await _CourseService.UpdateCourse(id, form);
            return new ApiResponse(HttpStatusCode.OK);
        }

        // Delete Course by ID
        [HttpDelete("{id}")]
        public async Task<ApiResponse> Delete(int id)
        {
            await _CourseService.DeleteCourse(id);
            return new ApiResponse(HttpStatusCode.NoContent);
        }
    }
}
