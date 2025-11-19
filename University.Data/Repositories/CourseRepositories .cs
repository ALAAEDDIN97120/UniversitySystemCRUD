using Microsoft.EntityFrameworkCore;
using University.Data.Contexts;
using University.Data.Entities;

namespace University.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly UniversityDbContext _context;
        public CourseRepository(UniversityDbContext context)
        {
            _context = context;
        }

        //============================== CRUD Operations ============================//

        // Get All Courses
        public async Task<List<Course>> GetAllAsync()
    {
        return await _context.Courses
            .AsNoTracking()
            .ToListAsync();
    }

        // Get Course by ID 
        public async Task<Course> GetByIdAsync(int courseId) 
        {
            if (courseId <= 0)
                throw new ArgumentOutOfRangeException(nameof(courseId), "CourseId must be greater than zero.");

            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");

            return course;

        }

        // Add new Course
        public async Task AddAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));


            course.CreatedTime = DateTime.Now;
            await _context.Courses.AddAsync(course);

        }

        // Delete Course by ID
        public async Task DeleteAsync(int courseId)
        {
            var course = await GetByIdAsync(courseId);
            if (course == null)
                throw new KeyNotFoundException($"Course with ID {courseId} not found.");

            _context.Courses.Remove(course);
        }

        // Update Course by ID
        public void Update(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            course.LastUpdateTime = DateTime.Now;
            _context.Courses.Update(course);
        }

        // Save changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }


    // Interface for Course Repository
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllAsync();
        Task<Course> GetByIdAsync(int courseId);
        Task AddAsync(Course course);
        void Update(Course course);
        Task DeleteAsync(int courseId);
        Task SaveChangesAsync();

    }
}
