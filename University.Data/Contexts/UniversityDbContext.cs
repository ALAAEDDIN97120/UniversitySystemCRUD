using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Data.Contexts.ClassMapping;
using University.Data.Entities;

namespace University.Data.Contexts
{
    public class UniversityDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StudentMapping());

        }


        private string ConnectionString = "Server=ALAEDDIN; Database=UniversitySystem; Trusted_Connection=True; MultipleActiveResultSets=True;TrustServerCertificate=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }


    }
}
