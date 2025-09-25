namespace University.Data.Entities
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Email { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        
    }
}
