using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using IK_Project.Infrastructure.Repositories.Concreates;
using IK_Project.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.ExpenseService
{
    public class ExpenseService : IExpenseService
    {
        IExpenseRepository _expenseRepository;
        IMapper _mapper;
        public ExpenseService(IExpenseRepository expenseRepository, IMapper mapper)
        {
            _expenseRepository = expenseRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<ExpenseListDTO>>> AllExpenses()
        {
            var expenses = await _expenseRepository.GetAllAsync();
            if (expenses.Count() <= 0)
            {
                return new ErrorDataResult<List<ExpenseListDTO>>("Sistemde gider bulunamadı");
            }
            return new SuccessDataResult<List<ExpenseListDTO>>(_mapper.Map<List<ExpenseListDTO>>(expenses), "Gider Listeleme başarılı");
        }

        public async Task<IDataResult<ExpenseCreateDTO>> Create(ExpenseCreateDTO expenseDTO)
        {
            var expense = _mapper.Map<Expense>(expenseDTO);
            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveChangeAsync();
            return new SuccessDataResult<ExpenseCreateDTO>(_mapper.Map<ExpenseCreateDTO>(expense), "Ekleme başarılı");
        }

        public async Task<IDataResult<ExpenseUpdateDTO>> Edit(ExpenseUpdateDTO expenseDTO)
        {
            var expense = await _expenseRepository.GetByIdAsync(expenseDTO.Id);
            if (expense == null)
            {
                return new ErrorDataResult<ExpenseUpdateDTO>("Gider bulunamadı");
            }          
            var updatedExpense = _mapper.Map(expenseDTO, expense);
            await _expenseRepository.UpdateAsync(updatedExpense);
            await _expenseRepository.SaveChangeAsync();
            return new SuccessDataResult<ExpenseUpdateDTO>(_mapper.Map<ExpenseUpdateDTO>(updatedExpense), "Güncelleme başarılı");
        }

        public async Task<IDataResult<ExpenseUpdateDTO>> GetById(Guid id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
            {
                return new ErrorDataResult<ExpenseUpdateDTO>("Gider bulunamadı");
            }
            return new SuccessDataResult<ExpenseUpdateDTO>(_mapper.Map<ExpenseUpdateDTO>(expense), "Gider başarıyla getirildi");
        }

        public async Task<IDataResult<List<ExpenseListDTO>>> GetDefaults(Expression<Func<Expense, bool>> expression)
        {
            var expense = await _expenseRepository.GetAllAsync(x=>x.Status!=Status.Deleted);
            if (expense.Count() <= 0)
            {
                return new ErrorDataResult<List<ExpenseListDTO>>("Sistemde gider bulunamadı");
            }
            return new SuccessDataResult<List<ExpenseListDTO>>(_mapper.Map<List<ExpenseListDTO>>(expense), "Gider listeleme başarılı");
        }

        public async Task<IResult> Remove(Guid id)
        {
            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
            {
                return new ErrorResult("Silinecek gider bulunamadı");
            }
            await _expenseRepository.DeleteAsync(expense);
            await _expenseRepository.SaveChangeAsync();
            return new SuccessResult("Silme Başarılı");
        }
        public async Task<IDataResult<List<ExpenseListDTO>>> GetDMExpense(Guid Id)
        {
            var expense = await _expenseRepository.GetDefault(x => x.DepartmantManagerId == Id && x.Status != Status.Deleted);
            if (expense == null)
            {
                return new ErrorDataResult<List<ExpenseListDTO>>("Expense listelenemedi.");
            }
            var expenseListDTOs = _mapper.Map<List<ExpenseListDTO>>(expense);

            return new SuccessDataResult<List<ExpenseListDTO>>(expenseListDTOs, "Expense listelendi.");
        }

        public async Task<IDataResult<List<ExpenseListDTO>>> GetEmployeeExpense(Guid Id)
        {
            var expenses = await _expenseRepository.GetDefault(x => x.EmployeeId == Id & x.Status != Status.Deleted);
            if (expenses == null)
            {
                return new ErrorDataResult<List<ExpenseListDTO>>("Expense listelenemedi.");
            }
            var expensedto = _mapper.Map<List<ExpenseListDTO>>(expenses);

            return new SuccessDataResult<List<ExpenseListDTO>>(expensedto, "Expense listelendi.");
        }
    }
}
