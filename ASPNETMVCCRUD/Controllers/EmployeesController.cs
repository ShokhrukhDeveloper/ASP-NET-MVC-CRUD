using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext _dbContext;

        public EmployeesController(MVCDemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
          var employee=await  _dbContext.Employees.ToListAsync();
            return View(employee);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeViewModel) 
        {
            var employee = new Employee()
            {
                Id =Guid.NewGuid(),
                Name = addEmployeeViewModel.Name,
                Email = addEmployeeViewModel.Email,
                Salary = addEmployeeViewModel.Salary,
                Department = addEmployeeViewModel.Department,
                DateOfBirth = addEmployeeViewModel.DateOfBirth
            };
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Guid Id)
        { 
            var employee=await _dbContext.Employees.FirstOrDefaultAsync(x=>x.Id==Id);
            if (employee!=null)
            {
                var viewModel = new UpdateEmployeeViewModel()
            {
                Id=Id,
                Name=employee.Name,
                Salary=employee.Salary,
                DateOfBirth=employee.DateOfBirth,
                Department=employee.Department,
                Email=employee.Email,

            };
                return await Task.Run(()=> View("View",viewModel));
            }
          
            return View(); 
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeViewModel model)
        {
            Console.WriteLine(model.Id);
            var employee=await _dbContext.Employees.FindAsync(model.Id);
            if(employee!= null)
            {
                employee.Name=model.Name;
                employee.Salary=model.Salary;
                employee.DateOfBirth=model.DateOfBirth;
                employee.Department=model.Department;
                employee.Email=model.Email;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        } 
    
    }
}
