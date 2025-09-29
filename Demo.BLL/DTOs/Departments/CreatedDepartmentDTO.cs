using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.DTOs.Departments
{
    public class CreatedDepartmentDTO
    {
        //[Required(ErrorMessage ="Name is required, please enter the name")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Code is Required Ya Hamada!!")]
        public string Code { get; set; } = null!;
        public string? Description { get; set; }

        [Display(Name = "Date Of Creation")]
        [DataType(DataType.Date)]
        public DateOnly CreationDate { get; set; }
    }
}
