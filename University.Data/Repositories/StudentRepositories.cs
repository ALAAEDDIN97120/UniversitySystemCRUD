using University.Data.Entities;
using University.Data.Contexts;

namespace University.Data.Repositories
{
    public class StudentRepositories : IStudentRepository
    {
        private readonly UniversityDbContext _context;
        public StudentRepositories(UniversityDbContext context)
        {
            _context = context;
        }
        public List<Student> GetAll()
        {
            return _context.Students.ToList();

        }

        public Student GetById(int StudentId) 
        {
            if (StudentId <= 0)
                throw new ArgumentOutOfRangeException(nameof(StudentId), "StudentId must be greater than zero.");

            var student = _context.Students.Find(StudentId);

            if (student == null)
                throw new KeyNotFoundException($"Student with ID {StudentId} not found.");
            return student;

        }

        public void Add(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            student.CreatedTime = DateTime.Now;
            _context.Students.Add(student);
           
        }

        public void Delete(int StudentId)
        {
            if (StudentId <= 0)
                throw new ArgumentOutOfRangeException(nameof(StudentId), "StudentId must be greater than zero.");

            _context.Students.Remove(GetById(StudentId));
            

        }
        public void Update(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));
            student.LastUpdateTime = DateTime.Now;
            _context.Students.Update(student);
          
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }

    public interface IStudentRepository
    {
        Student GetById(int StudentId);
        List<Student> GetAll();
        void Add(Student student);
        void Update(Student student);
        void Delete(int StudentId);
        void SaveChanges();

    }
}
