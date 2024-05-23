using Atelier5.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static Atelier5.Model.Repositories.EmployeeRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Atelier5.Model.Repositories
{
    public class EmployeeRepository : IEmployeRpository
    {

        private readonly AppDbContext appDbContext;
        public EmployeeRepository(AppDbContext appDbContext)
        {

            this.appDbContext = appDbContext;

        }
        public async Task<IEnumerable<Employee>> GetEmployee()
        {
            return await appDbContext.Employees.ToListAsync();
        }
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await appDbContext.Employees.ToListAsync();
        }
        public async Task<IEnumerable<Employee>> Search(string name, Gender? gender)
        {
            IQueryable<Employee> query = appDbContext.Employees;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name));
            }
            if (gender != null)
            {
                query = query.Where(e => e.Gender == gender);
            }

            return await query.ToListAsync();
        } 
        public async Task<Employee> GetEmployee(int employeeId)
        {

            return await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var result = await appDbContext.Employees.AddAsync(employee);
            await appDbContext.SaveChangesAsync();
            return result.Entity;
        }

        //public Task<IEnumerable<Employee>> GetEmployee()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Employee> UpdateEmployee(Employee employee) {

            var result = await appDbContext.Employees
            .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

            if (result != null)
            {

                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateOfBrith = employee.DateOfBrith;
                result.Gender = employee.Gender;
                result.DepartmentId = employee.DepartmentId;
                result.PhotoPath = employee.PhotoPath;

                await appDbContext.SaveChangesAsync();

                return result;
            }
            return null;
        }

        public async Task<Employee> DeleteEmployee(int employeeId)
        {

            var result = await appDbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            if (result != null)
            {
                appDbContext.Employees.Remove(result);
                await appDbContext.SaveChangesAsync();
                return result;

            }

            return null;

        }

        //public Task<Employee> DeleteEmployee(int employeeId)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<Employee> GetEmployeeByEmail(string email)
        {
            throw new NotImplementedException();
        }
    }
} 
