using Demo.BLL.DTOs.Employees;
using Demo.BLL.Services.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    [Authorize]
    // Inheritance : EmployeeController is Controller
    // Composition : EmployeeController has a EmployeeService
    public class EmployeeController : Controller
    {
        #region Services
        private readonly IEmployeeService _employeeService;
        //private readonly IDepartmentService _departmentService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(
            IEmployeeService employeeService,
            // IDepartmentService departmentService,
            ILogger<EmployeeController> logger, IWebHostEnvironment environment)
        {
            _employeeService = employeeService;
            // _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
        }
        #endregion

        #region Index
        [HttpGet]  // Get: /Employee?Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            ViewData["ControllerName"] = "Employee";
            var employees =await _employeeService.GetAllEmployeesAsync(SearchValue);
            return View(employees);
        }
        #endregion

        #region Create
        [HttpGet]  // Get: /Employee?Create
        public IActionResult Create(/*[FromServices] IDepartmentService departmentService*/ )
        {
            //ViewData["Department"] = departmentService.GetAllDepartments();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Action Filter
        public async Task<IActionResult> Create(CreatedEmployeeDTO employee)
        {

            if (!ModelState.IsValid)  // Server Side Validation
                return View(employee);

            //if (string.IsNullOrWhiteSpace(employee.Name) || string.IsNullOrWhiteSpace(employee.id))
            //{
            //    ModelState.AddModelError("", "Name And Code Are Required.");
            //    return View(employee);
            //}

            var message = string.Empty;
            try
            {
                var result =await _employeeService.CreateEmployeeAsync(employee);
                if (result > 0)
                    return RedirectToAction(nameof(Index));
                else
                {
                    message = "Employee is not Created";
                    ModelState.AddModelError(string.Empty, message);
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                //1. Log Exception
                _logger.LogError(ex, ex.ToString());
                //2. Set Message
                message = _environment.IsDevelopment() ? ex.ToString() : "An Error Occured During Creating Employee";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(employee);
        }
        #endregion

        #region Details
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();  // 400

            var employee =await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee is null)
                return NotFound();    // 404

            return View(employee);
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Edit(int? id  /*IDepartmentService departmentService*/)
        {
            if (!id.HasValue)
                return BadRequest();

            var employee =await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (employee is null)
                return NotFound();

            //ViewData["Department"] = departmentService.GetAllDepartments();


            return View(new UpdatedEmployeeDTO()
            {
                Id = employee.Id,
                Name = employee.Name,
                Address = employee.Address,
                Email = employee.Email,
                Age = employee.Age,
                Salary = employee.Salary,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                EmployeeType = employee.EmployeeType,
                Gender = employee.Gender,
                HiringDate = employee.HiringDate
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Action Filter
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatedEmployeeDTO employee)
        {
            if (!ModelState.IsValid)
                return View(employee);
            var message = string.Empty;

            try
            {
                var updated =await _employeeService.UpdateEmployeeAsync(employee) > 0;

                if (updated)
                    return RedirectToAction(nameof(Index));

                message = "An Error Occured During Updating Employee:";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Updating Employee";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(employee);
        }
        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var employee =await _employeeService.GetEmployeeByIdAsync(id.Value);

            if (employee is null)
                return NotFound();

            return View(employee);
        }

        [ValidateAntiForgeryToken] //Action Filter
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var message = string.Empty;
            try
            {
                var deleted =await _employeeService.DeleteEmployeeAsync(id);

                if (deleted)
                    return RedirectToAction(nameof(Index));

                message = "An Error Occured During Deleting This Department :";

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);

                message = _environment.IsDevelopment() ? ex.Message : "An Error Occured During Deleting Employee";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
