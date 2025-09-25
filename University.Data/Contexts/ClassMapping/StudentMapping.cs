using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using University.Data.Entities;

namespace University.Data.Contexts.ClassMapping
{
    internal class StudentMapping : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(s => s.StudentId);
            builder.Property(s => s.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(s => s.LastName).IsRequired().HasMaxLength(50);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(100);

        }
    }
}
