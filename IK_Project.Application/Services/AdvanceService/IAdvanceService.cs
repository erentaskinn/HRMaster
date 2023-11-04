using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.AdvanceService
{
    public interface IAdvanceService
    {
		Task<IResult> Create(AdvanceCreateDTO advanceCreateDTO);
		Task<IResult> Edit(AdvanceUpdateDTO advanceUpdateDTO);
		Task<IResult> Remove(Guid id);

		Task<IDataResult<List<AdvanceListDTO>>> GetDefaults(Expression<Func<Advance, bool>> expression);
        Task<IDataResult<List<AdvanceListDTO>>> AllAdvances();
        Task<IDataResult<AdvanceUpdateDTO>> GetById(Guid id);
        Task<IDataResult<List<AdvanceListDTO>>> GetDMAdvance(Guid Id);
         Task<IDataResult<List<AdvanceListDTO>>> GetEmployeeAdvance(Guid Id);
    }
}
