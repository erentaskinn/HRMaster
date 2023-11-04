using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System.Linq.Expressions;

namespace IK_Project.Application.Services.CompanyService
{
    public interface ICompanyService
    {

        Task<IDataResult<CompanyCreateDTO>> Create(CompanyCreateDTO companyDTO);
        Task<IDataResult<CompanyUpdateDTO>> Edit(CompanyUpdateDTO companyDTO);
        Task<IResult> Remove(Guid id);
        Task<IDataResult<List<CompanyListDTO>>> GetDefaults(Expression<Func<Company, bool>> expression);
        Task<IDataResult<List<CompanyListDTO>>> AllCompanies();
        Task<IDataResult<CompanyDTO>> GetById(Guid id);
        Task<IDataResult<CompanyDTO>> EditToggle(CompanyDTO companyDTO, bool newstatus);
        Task<IDataResult<CompanyDTO>> GetByIdDepartman(Guid DepartmantId);
        Task<IDataResult<CompanyDTO>> GetCompany(Expression<Func<Company, bool>> expression);

    }
}
