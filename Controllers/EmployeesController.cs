using Atelier5.Data;
using Atelier5.Model;
using Atelier5.Model.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;

namespace Atelier5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeRpository employeeRepository;
        public EmployeesController(IEmployeRpository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        // GET: EmployeesController
        //public ActionResult Index()
        //{
        //    return View () ;
        //}

        //// GET: EmployeesController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: EmployeesController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: EmployeesController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: EmployeesController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: EmployeesController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: EmployeesController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: EmployeesController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await employeeRepository.GetEmployees());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");

            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await employeeRepository.GetEmployee(id);
                if (result == null) return NotFound();
                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database");

            }
        }

        [HttpGet("{search}")]

        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try {

                var result = await employeeRepository.Search(name, gender);

                if (result.Any())
                {
                    return Ok(result);
                }



                return NotFound();
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)

        {
            try
            {
                if (employee == null)
                    return BadRequest();
                var createdEmployee = await employeeRepository.AddEmployee(employee);

                return CreatedAtAction(nameof(GetEmployee),
                new { id = createdEmployee.EmployeeId }, createdEmployee);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating new employee record");
            }
        }

        //public async Task<Employee> GetEmployeeByEmail(string email)
        //{
        //    return await AppDbContext. .FirstOrDefaultAsync(e => e.Email == email);

        //}
        [HttpPut("{id:int}")]

        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {

            try
            {

                if (id != employee.EmployeeId)
                    return BadRequest("Employee ID mismatch");

                var employeeToUpdate = await employeeRepository.GetEmployee(id);

                if (employeeToUpdate == null)
                    return NotFound($"Employee with Id = {id} not found");

                return await employeeRepository.UpdateEmployee(employee);

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
            }
        }


        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {

                if (employee == null)
                {

                    return BadRequest();
                }

                else
                // Add custom model validation error
                {

                    var emp = await employeeRepository.GetEmployeeByEmail(employee.Email);
                    if (emp != null)
                    {

                        ModelState.AddModelError("email", "Employee email already in use");
                        return BadRequest(ModelState);
                    }

                    else
                    {
                        var createdEmployee = await employeeRepository.AddEmployee(employee);

                        return CreatedAtAction(nameof(GetEmployee), new { id = createdEmployee.EmployeeId },
                            createdEmployee);

                    }
                }
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");

            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        { try
            {

                var employeeToDelete = await employeeRepository.GetEmployee(id);

                if (employeeToDelete == null)
                {

                    return NotFound($"Employee with Id = {id} not found");
                }

                return await employeeRepository.DeleteEmployee(id);
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data");
            }
        }
        [HttpGet("{search}")]

        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try
            {

                var result = await employeeRepository.Search(name, gender);

                if (result.Any())
                {
                    return Ok(result);
                }



                return NotFound();
            }

            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }


        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)

        {
            try
            {
                if (employee == null)
                    return BadRequest();
                var createdEmployee = await employeeRepository.AddEmployee(employee);

                return CreatedAtAction(nameof(GetEmployee),
                new { id = createdEmployee.EmployeeId }, createdEmployee);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error creating new employee record");
            }
        }

        //public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        //{

        //    try
        //    {
        //        var result = await employeeRepository.Search(name, gender);

        //        if (result.Any())
        //        {

        //            return Ok(result);
        //        }
        //        return NotFound();

        //    }



        //    catch (Exception)
        //    {

        //        return StatusCode(StatusCodes.Status500InternalServerError,
        //        "Error retrieving data from the database");

        //    }
        //}

    }
}