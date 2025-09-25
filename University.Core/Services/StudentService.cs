using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Core.DTOs;
using University.Core.Exceptions;
using University.Core.Forms;
using University.Core.Validations;
using University.Data.Entities;
using University.Data.Repositories;
namespace University.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepositories;
        private readonly ILogger<StudentService> _logger;


        public StudentService(IStudentRepository studentRepositories , ILogger<StudentService> logger)
        {
            _studentRepositories = studentRepositories;
            _logger = logger;
        }

        public void CreateStudent(CreateStudentForm form)
        {
            _logger.LogInformation("Creating a new student.");
            // Validation
            if (form == null)
                throw new ArgumentNullException(nameof(form), "Form cannot be null.");
           
            var validation = FormValidator.Validate(form) ;
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            // Logic to create a new student
            var newStudent = new Student
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email
            };

            // Save to repository
            _studentRepositories.Add(newStudent);
            _studentRepositories.SaveChanges();

        }

        public void DeleteStudent(int studentId)
        {
            _logger.LogInformation($"Deleting student with ID: {studentId}");
            var srudent = _studentRepositories.GetById(studentId);
            if (srudent == null)
                throw new KeyNotFoundException($"Student with ID {studentId} not found.");
            _studentRepositories.Delete(studentId);
            _studentRepositories.SaveChanges();

        }

        public List<StudentDTO> GetAllStudents()
        {
            _logger.LogInformation("Fetching all students.");
            var allstudent = _studentRepositories.GetAll();

            return allstudent.Select(student => new StudentDTO()
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email
            }).ToList();

        }

        public StudentDTO GetStudentById(int studentId)
        {
            _logger.LogInformation($"Fetching student with ID: {studentId}");
            var student = _studentRepositories.GetById(studentId);
            if (student == null)
                throw new NotFoundException($"Student with ID {studentId} not found.");
            return new StudentDTO()
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email
            };

        }

        public void UpdateStudent(int studentId, UpdateStudentForm form)
        {
            _logger.LogInformation($"Updating student with ID: {studentId}");
            var student = _studentRepositories.GetById(studentId);
            if (student == null)
                throw new NotFoundException($"Student with ID {studentId} not found.");

            // Validation
            if (form == null)
                throw new BusinessException("Form cannot be null.");

            var validation = FormValidator.Validate(form);
            if (!validation.IsValid)
                throw new BusinessException(validation.Errors);

            // Update fields
            student.FirstName = form.FirstName;
            student.LastName = form.LastName;

            // Save changes
            _studentRepositories.Update(student);
            _studentRepositories.SaveChanges();
        }
    }

    public interface IStudentService
    {
        StudentDTO GetStudentById(int studentId);
        List<StudentDTO> GetAllStudents();
        void CreateStudent(CreateStudentForm form);
        void UpdateStudent(int studentId, UpdateStudentForm form);
        void DeleteStudent(int studentId);
   
    }

}
