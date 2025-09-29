using Demo.DAL.Entities.Departments;
using Demo.DAL.Persistence.Repositories._Genaric;

namespace Demo.DAL.Persistence.Repositories.Departments
{
    public interface IDepartmentRepository : IGenaricRepository<Department>
    {
        //IEnumerable<Department> GetSpecificDepartment();
    }
}
