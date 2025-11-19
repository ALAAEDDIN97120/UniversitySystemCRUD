
namespace University.Data.Entities
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public int CourseWeight { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
