using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels.Departments
{
    public class DepartmentViewModel
    {

        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "code is required ya hamada!!")]
        public string Code { get; set; } = null!;
        public string? Description { get; set; }

        [Display(Name = "Creation Date")]
        public DateOnly CreationDate { get; set; }
    }
}
