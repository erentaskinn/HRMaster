using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.ExpenseService
{
    public interface IExpenseService
    {
        Task<IDataResult<ExpenseCreateDTO>> Create(ExpenseCreateDTO expenseDTO);
        Task<IDataResult<ExpenseUpdateDTO>> Edit(ExpenseUpdateDTO expenseDTO);
        Task<IResult> Remove(Guid id);
        Task<IDataResult<List<ExpenseListDTO>>> GetDefaults(Expression<Func<Expense, bool>> expression);
        Task<IDataResult<List<ExpenseListDTO>>> AllExpenses();
        Task<IDataResult<ExpenseUpdateDTO>> GetById(Guid id);
        Task<IDataResult<List<ExpenseListDTO>>> GetDMExpense(Guid Id);
        Task<IDataResult<List<ExpenseListDTO>>> GetEmployeeExpense(Guid Id);
    }
}
