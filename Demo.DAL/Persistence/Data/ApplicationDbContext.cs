using Demo.DAL.Entities.Departments;
using Demo.DAL.Entities.Employees;
using Demo.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Demo.DAL.Persistence.Data
{
    //ApplicationUser : IdentityUser
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        // Repo => Application DbContext
        // DepartmentRepo => Open Connection With DB
        // EmployeeRepo => Open Connection With DB
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        ///protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        ///{
        ///    optionsBuilder.UseSqlServer("Server=.; Database=MVCApplication; Trusted_Connection=True; TrustServerCertificate=True;");
        ///}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);    
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());  //Apply All Configurations Classes
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //IdentityUser, IdentityRole
        //public DbSet<IdentityUser> Users { get; set; }
        //public DbSet<IdentityRole> Roles { get; set; }

    }
}
