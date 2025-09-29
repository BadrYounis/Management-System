using Demo.BLL.DTOs.Departments;
using Demo.DAL.Entities.Departments;
using Demo.DAL.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Services.Departments
{
    public class DepartmentsServices : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        //public DepartmentsServices(IDepartmentRepository departmentRepository)
        //{
        //    _departmentRepository = departmentRepository;
        //}
        public DepartmentsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            var departments = await _unitOfWork.DepartmentRepository
                              .GetAllQueryable()
                              .Where(d => !d.IsDeleted)
                              .Select(department => new DepartmentDTO
                              {
                                  Id = department.Id,
                                  Name = department.Name,
                                  Code = department.Code,
                                  CreationDate = department.CreationDate
                              })
                              .AsNoTracking()
                              .ToListAsync(); // Execute query before returning

            return departments;

            /*
            // Alternative approach using `yield return`
            var departments = _departmentRepository.GetAll(true);
            foreach (var department in departments)
            {
                yield return new DepartmentDTO()
                {
                    Id = department.Id,
                    Name = department.Name,
                    Code = department.Code,
                    CreationDate = department.CreationDate
                };
            }
            */
        }
        public async Task<DepartmentDetailsDTO?> GetDepartmentByIdAsync(int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department is not null)
            {     //department != null, department is {}
                return new DepartmentDetailsDTO()
                {
                    Id = department.Id,
                    Code = department.Code,
                    Name = department.Name,
                    CreatedBy = department.CreatedBy,
                    CreatedOn = department.CreatedOn,
                    CreationDate = department.CreationDate,
                    LastModifiedBy = department.LastModifiedBy,
                    Description = department.Description,
                    LastModifiedOn = department.LastModifiedOn,
                    IsDeleted = department.IsDeleted
                };
            }
            return null;
        }
        public async Task<int> CreateDepartmentAsync(CreatedDepartmentDTO departmentDTO)
        {
            var department = new Department()
            {
                Code = departmentDTO.Code,
                Name = departmentDTO.Name,
                Description = departmentDTO.Description,
                CreationDate = departmentDTO.CreationDate,
                CreatedBy = 1,
                CreatedOn = DateTime.UtcNow,
                LastModifiedBy = 1
            };
            _unitOfWork.DepartmentRepository.Add(department);
            return await _unitOfWork.CompleteAsync();
        }
        public async Task<int> UpdateDepartmentAsync(UpdatedDepartmentDTO department)
        {
            ///var existingDepartment = await _unitOfWork.DepartmentRepository.GetByIdAsync(departmentDTO.Id);
            ///if (existingDepartment is null)
            ///    return 0;
            ///
            ///
            /// //Id = departmentDTO.Id,
            /// existingDepartment.Code = departmentDTO.Code;
            /// existingDepartment.Name = departmentDTO.Name;
            /// existingDepartment.Description = departmentDTO.Description;
            /// existingDepartment.CreationDate = departmentDTO.CreationDate;
            /// existingDepartment.LastModifiedBy = 1;

            var departmentUpdated = new Department()
            {
                Id = department.Id,
                Code = department.Code,
                Description = department.Description,
                Name = department.Name,
                CreationDate = department.CreationDate,
                LastModifiedBy = 1,   //UserId
                CreatedBy = 1,    //UserId
                LastModifiedOn = DateTime.UtcNow
            };
            _unitOfWork.DepartmentRepository.Update(departmentUpdated);
            return await _unitOfWork.CompleteAsync();
        }
        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var departmentRepo = _unitOfWork.DepartmentRepository;
            var department = await departmentRepo.GetByIdAsync(id);
            if (department is { })
                departmentRepo.Delete(department);
            return await _unitOfWork.CompleteAsync() > 0;
        }
    }
}
