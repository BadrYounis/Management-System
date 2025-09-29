using Demo.DAL.Entities.Departments;
using Demo.DAL.Persistence.Data;
using Demo.DAL.Persistence.Repositories._Genaric;
using Demo.DAL.Persistence.Repositories.Employees;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL.Persistence.Repositories.Departments
{
    public class DepartmentRepository : GenaricRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}
