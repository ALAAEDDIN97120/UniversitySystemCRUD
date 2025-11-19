using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Data.Entities;

namespace University.Data.Contexts.ClassMapping
{
    public class CourseMapping : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(s => s.CourseId);
            builder.Property(s => s.CourseTitle).IsRequired().HasMaxLength(50);
            builder.Property(s => s.CourseDescription).IsRequired().HasMaxLength(250);
            builder.Property(s => s.CourseWeight).IsRequired();

        }
    }
}
