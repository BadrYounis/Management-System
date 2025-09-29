using AutoMapper;
using Demo.BLL.DTOs.Departments;
using Demo.DAL.Entities.Departments;
using Demo.PL.ViewModels.Departments;

namespace Demo.PL.Mapping.Profiles
{
    //Configurations that's belong to autoMapper
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Employee Module

            #endregion

            #region Department Module
            CreateMap<DepartmentViewModel, CreatedDepartmentDTO>();
                //.ForMember(dest=>dest.Name, config=>config.MapFrom(src=>src.DepartmentName));

            CreateMap<DepartmentDetailsDTO, DepartmentViewModel>();

            CreateMap<DepartmentViewModel, UpdatedDepartmentDTO>();
            #endregion
        }
    }
}
