using AutoMapper;
using Demo.BLL.DTOs.Departments;
using Demo.BLL.Services.Departments;
using Demo.PL.ViewModels.Departments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    //[AllowAnonymous]
    [Authorize] //Authenticated is Authorized
    //Department Controller : Inheritance [is a controller]
    //Department Controller : Composition [has a department service]
    public class DepartmentController : Controller
    {
        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IMapper mapper, IWebHostEnvironment environment)
        {
            _departmentService = departmentService;
            _logger = logger;
            _mapper = mapper;
            _environment = environment;
        }
        #endregion

        #region Index
        //viewData , viewBag
        [HttpGet]  // Get: /Department?Index
        public async Task<IActionResult> Index()
        {
            //view Dictonary : pass data from controller [Action] to view 

            //1-viewData
            //ViewData["Obj"] = "Hello view Data";

            //2.viewbag
            // ViewBag.Obj = "Hello View Bag";
            ViewData["ControllerName"] = "Department";
            var departments = await _departmentService.GetAllDepartmentsAsync();

            return View(departments);
        }
        #endregion

        #region Create
        [HttpGet]  // Get: /Department?Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Action Filter
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)  // Server Side Validation
                return View(departmentVM);

            if (string.IsNullOrWhiteSpace(departmentVM.Name) || string.IsNullOrWhiteSpace(departmentVM.Code))
            {
                ModelState.AddModelError("", "Name And Code Are Required.");
                return View(departmentVM);
            }

            var message = string.Empty;
            try
            {
                // DepartmentViewModel => CreatedDepartmentDTO [Configurations]
                var CreatedDepartment = _mapper.Map<DepartmentViewModel, CreatedDepartmentDTO>(departmentVM);
                ///var CreatedDepartment = new CreatedDepartmentDTO()
                ///{
                ///    Code = departmentVM.Code,
                ///    Name = departmentVM.Name,
                ///    Description = departmentVM.Description,
                ///    CreationDate = departmentVM.CreationDate
                ///};
                var result = await _departmentService.CreateDepartmentAsync(CreatedDepartment);

                if (result > 0)
                    TempData["Message"] = "Department is created Successfully";

                else
                {
                    message = "Department is not Created";
                    ModelState.AddModelError(string.Empty, message);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //1. Log Exception
                _logger.LogError(ex, ex.ToString());
                //2. Set Message
                message = _environment.IsDevelopment() ? ex.ToString() : "An Error Occured During Creating Department";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();  // 400

            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);
            if (department is null)
                return NotFound();    // 404

            return View(department);
        }
        #endregion

        #region Update
        [HttpGet]  // GET: /Department/Edit 
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

            if (department is null)
                return NotFound();

            var departmentVM = _mapper.Map<DepartmentDetailsDTO, DepartmentViewModel>(department);
            return View(departmentVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Action Filter
        public async Task<IActionResult> Edit([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;

            try
            {
                var departmentToUpdate = _mapper.Map<DepartmentViewModel,UpdatedDepartmentDTO>(departmentVM);
                departmentToUpdate.Id = id;
                var updated = await _departmentService.UpdateDepartmentAsync(departmentToUpdate) > 0;
                if (updated)
                    return RedirectToAction(nameof(Index));

                message = "An Error Occured During Updating Department:";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Updating Department";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var department = await _departmentService.GetDepartmentByIdAsync(id.Value);

            if (department is null)
                return NotFound();

            return View(department);
        }
        [ValidateAntiForgeryToken] //Action Filter
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = await _departmentService.DeleteDepartmentAsync(id);

                if (deleted)
                    return RedirectToAction(nameof(Index));

                message = "An Error Occured During Deleting This Department :";

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);

                message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Deleting Department";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
