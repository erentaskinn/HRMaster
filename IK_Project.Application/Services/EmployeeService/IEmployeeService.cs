using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.EmployeeService
{
    public interface IEmployeeService
    {
        Task<IDataResult<EmployeeCreateDTO>> Create(EmployeeCreateDTO employeeDto);
        Task<IDataResult<EmployeeUpdateDTO>> Edit(EmployeeUpdateDTO employeeDto);
        Task<IResult> Remove(Guid id);
        Task<IDataResult<List<EmployeeListDTO>>> GetDefaults(Expression<Func<Employee, bool>> expression);
        Task<IDataResult<List<EmployeeListDTO>>> AllEmployees();
        Task<IDataResult<List<EmployeeListDTO>>> AllActiveEmployees();
        Task<IDataResult<EmployeeDTO>> GetById(Guid id);
        Task<IDataResult<EmployeeDTO>> GetAsync(string id);
    }
}
