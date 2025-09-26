using Microsoft.Extensions.Logging;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms.CourseForms;
using University.Core.Validations;
using University.Data.Entities;
using University.Data.Repositories;
namespace University.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _CourseRepositories;
        private readonly ILogger<CourseService> _logger;


        public CourseService(ICourseRepository CourseRepositories , ILogger<CourseService> logger)
        {
            _CourseRepositories = CourseRepositories;
            _logger = logger;
        }
        //============================== Business Logic for Courses ============================//

        // Create a new Course
        public async Task CreateCourse(CreateCourseForm form)
        {
            _logger.LogInformation("Creating a new Course.");

            // Validation 
            if (form == null)
                throw new ArgumentNullException(nameof(form), "Form cannot be null.");
           
            var validation = FormValidator.Validate(form) ;
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            // Logic to create a new Course
            var newCourse = new Course
            {
                CourseTitle = form.CourseTitle,
                CourseDescription = form.CourseDescription,
                CourseWeight = form.CourseWeight
            };

            // Save to repository
            await _CourseRepositories.AddAsync(newCourse);
            await _CourseRepositories.SaveChangesAsync();

        }

        // Delete Course by ID
        public async Task DeleteCourse(int CourseId)
        {
            _logger.LogInformation($"Deleting Course with ID: {CourseId}");
            var course = await _CourseRepositories.GetByIdAsync(CourseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {CourseId} not found.");
            await _CourseRepositories.DeleteAsync(CourseId);
            await _CourseRepositories.SaveChangesAsync();

        }

        // Get all Courses
        public async Task<List<CourseDTO>> GetAllCourses()
        {
            _logger.LogInformation("Fetching all Courses.");

            var allCourse = await _CourseRepositories.GetAllAsync();
            return allCourse.Select(Course => new CourseDTO()
            {
                CourseId = Course.CourseId,
                CourseTitle = Course.CourseTitle,
                CourseDescription = Course.CourseDescription,
                CourseWeight = Course.CourseWeight
            }).ToList();

        }

        // Get Course by ID
        public async Task<CourseDTO> GetCourseById(int courseId)
        {
            _logger.LogInformation($"Fetching Course with ID: {courseId}");
            var Course = await _CourseRepositories.GetByIdAsync(courseId);
            if (Course == null)
                throw new NotFoundException($"Course with ID {courseId} not found.");
            return new CourseDTO()
            {
                CourseId = Course.CourseId,
                CourseTitle = Course.CourseTitle,
                CourseDescription = Course.CourseDescription,
                CourseWeight = Course.CourseWeight
            };

        }
        
        // Update Course by ID
        public async Task UpdateCourse(int CourseId, UpdateCourseForm form)
        {
            _logger.LogInformation($"Updating Course with ID: {CourseId}");
            var course = await _CourseRepositories.GetByIdAsync(CourseId);
            if (course == null)
                throw new NotFoundException($"Course with ID {CourseId} not found.");

            // Validation
            if (form == null)
                throw new BusinessException("Form cannot be null.");

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            // Update fields
            course.CourseTitle = form.CourseTitle;
            course.CourseDescription = form.CourseDescription;
            course.CourseWeight = form.CourseWeight;


            // Save changes
            _CourseRepositories.Update(course);
            await _CourseRepositories.SaveChangesAsync();

        }
    }

    public interface ICourseService
    {
        Task<CourseDTO> GetCourseById(int CourseId);
        Task<List<CourseDTO>> GetAllCourses();
        Task CreateCourse(CreateCourseForm form);
        Task UpdateCourse(int CourseId, UpdateCourseForm form);
        Task DeleteCourse(int CourseId);

    }

}
