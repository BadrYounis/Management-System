using Demo.BLL.Common.Services.Attachments;
using Demo.BLL.DTOs.Employees;
using Demo.BLL.Services.Employee;
using Demo.DAL.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        //private readonly IEmployeeRepository _employeeRepository;
        //
        //public EmployeeService(IEmployeeRepository employeeRepository)
        //{
        //    _employeeRepository = employeeRepository;
        //}
        public EmployeeService(IUnitOfWork unitOfWork, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }
        public async Task<int> CreateEmployeeAsync(CreatedEmployeeDTO employeeDTO)
        {
            DAL.Entities.Employees.Employee employee = new()
            {
                Name = employeeDTO.Name,
                Age = employeeDTO.Age,
                Address = employeeDTO.Address,
                Salary = employeeDTO.Salary,
                IsActive = employeeDTO.IsActive,
                Email = employeeDTO.Email,
                Phone = employeeDTO.PhoneNumber,
                HiringDate = employeeDTO.HiringDate,
                Gender = employeeDTO.Gender,
                EmployeeType = employeeDTO.EmployeeType,
                DepartmentId = employeeDTO.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow,
            };
            if (employeeDTO.Image is not null)
                employee.Image =await _attachmentService.UplaodAsync(employeeDTO.Image, "images");

            _unitOfWork.EmployeeRepository.Add(employee);
            return await _unitOfWork.CompleteAsync();   //Number of rows affected
        }
        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var employeeRepo = _unitOfWork.EmployeeRepository;
            var employee = await employeeRepo.GetByIdAsync(id);

            if (employee is { })    //(employee != null), (employee is not null)
                employeeRepo.Delete(employee);
            return await _unitOfWork.CompleteAsync() > 0;
        }
        public async Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync(string SearchValue)
        {
            return await _unitOfWork.EmployeeRepository
                           .GetAllQueryable()
                           .Where(E => !E.IsDeleted && (string.IsNullOrEmpty(SearchValue) || E.Name.ToLower().Contains(SearchValue.ToLower())))
                           .Include(E => E.Department)
                           .Select(employee => new EmployeeDTO()
                           {
                               Id = employee.Id,
                               Name = employee.Name,
                               Age = employee.Age,
                               IsActive = employee.IsActive,
                               Salary = employee.Salary,
                               Email = employee.Email,
                               Gender = employee.Gender.ToString(),
                               EmployeeType = employee.EmployeeType.ToString(),
                               Department = employee.Department.Name,
                               Image = employee.Image
                           }).ToListAsync();
        }
        /// public IEnumerable<EmployeeDTO> GetAllEmployees(string SearchValue)
        /// {
        ///     var employees = _employeeRepository
        ///                    .GetIQueryable()
        ///                    .Where(E => !E.IsDeleted)
        ///                    .Select(employee => new EmployeeDTO()
        ///                    {
        ///                        Id = employee.Id,
        ///                        Name = employee.Name,
        ///                        Age = employee.Age,
        ///                        IsActive = employee.IsActive,
        ///                        Salary = employee.Salary,
        ///                        Email = employee.Email,
        ///                        Gender = employee.Gender.ToString(),
        ///                        EmployeeType = employee.EmployeeType.ToString()
        ///                    });
        ///     return employees;
        /// }
        public async Task<EmployeeDetailsDTO?> GetEmployeeByIdAsync(int id)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetByIdAsync(id);

            if (employee is { })
            {
                return new EmployeeDetailsDTO
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    PhoneNumber = employee.Phone,
                    HiringDate = employee.HiringDate,
                    Gender = employee.Gender,
                    EmployeeType = employee.EmployeeType,
                    Department = employee.Department?.Name ?? "No Department", //Once Access Department[NAME]
                    Image = employee.Image
                };
            }
            return null!;
        }
        /// public EmployeeDTO? GetEmployeeById(int id)
        /// {
        ///     var employee = _employeeRepository.Get(id);         
        ///     if (employee is { })
        ///         return new EmployeeDTO()
        ///         {
        ///             Id = employee.Id,
        ///             Name = employee.Name,
        ///             Age = employee.Age,
        ///             Address = employee.Address,
        ///             IsActive = employee.IsActive,
        ///             Salary = employee.Salary,
        ///             Email = employee.Email,
        ///             PhoneNumber = employee.Phone,
        ///             HiringDate = employee.HiringDate,
        ///             Gender = employee.Gender.ToString(),
        ///             EmployeeType = employee.EmployeeType.ToString()
        ///         };
        ///     else
        ///         return null;
        /// }
        public async Task<int> UpdateEmployeeAsync(UpdatedEmployeeDTO employeeDTO)
        {
            var employee = new DAL.Entities.Employees.Employee()
            {
                Id = employeeDTO.Id,
                Name = employeeDTO.Name,
                Age = employeeDTO.Age,
                Address = employeeDTO.Address,
                IsActive = employeeDTO.IsActive,
                Salary = employeeDTO.Salary,
                Email = employeeDTO.Email,
                Phone = employeeDTO.PhoneNumber,
                HiringDate = employeeDTO.HiringDate,
                Gender = employeeDTO.Gender,
                EmployeeType = employeeDTO.EmployeeType,
                DepartmentId = employeeDTO.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.UtcNow
            };
            _unitOfWork.EmployeeRepository.Update(employee);
            return await _unitOfWork.CompleteAsync();
        }      
    }
}
