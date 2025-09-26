using System.ComponentModel.DataAnnotations;


namespace University.Core.Forms.CourseForms
{
    public class CreateCourseForm
    {
        [Required]
        public string CourseTitle { get; set; }
        [Required]
        public string CourseDescription { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "CourseWeight must be between 0 and 100.")]
        public int CourseWeight { get; set; }
    }
}
