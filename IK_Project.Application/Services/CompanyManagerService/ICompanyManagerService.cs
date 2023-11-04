using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.CompanyManagerService
{
    public  interface ICompanyManagerService
    {
		Task<IDataResult<CompanyManagerCreateDTO>> Create(CompanyManagerCreateDTO companyManagerCreateDTO);
		Task<IDataResult<CompanyManagerUpdateDTO>> Edit(CompanyManagerUpdateDTO companyManagerUpdateDTO);
		Task<IResult> Remove(Guid id);

		Task<IDataResult<List<CompanyManagerListDTO>>> GetDefaults(Expression<Func<CompanyManager, bool>> expression);
		Task<IDataResult<List<CompanyManagerListDTO>>> AllManagers();

		Task<IDataResult<CompanyManagerUpdateDTO>> GetById(Guid id);
        Task<bool> IsManagerExists(Guid managerId);

		Task<IDataResult<CompanyManagerUpdateDTO>> GetAsync(string id);

    }
}
