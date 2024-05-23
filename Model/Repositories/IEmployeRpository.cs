namespace Atelier5.Model.Repositories
{
    public interface IEmployeRpository
    {
     
       Task<IEnumerable<Employee>> GetEmployees();
     
       Task<Employee> GetEmployee(int employeeId);
    
       Task<Employee> AddEmployee(Employee employee);
     
       Task<Employee> UpdateEmployee(Employee employee);
     
      Task<Employee> DeleteEmployee(int employeeId);

      Task<Employee> GetEmployeeByEmail(string email);

       


}
}
