using Demo.BLL.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Services.Employee
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync(string SearchValue);
        Task<EmployeeDetailsDTO?> GetEmployeeByIdAsync(int id);
        Task<int> CreateEmployeeAsync(CreatedEmployeeDTO employeeDTO);
        Task<int> UpdateEmployeeAsync(UpdatedEmployeeDTO employeeDTO);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
