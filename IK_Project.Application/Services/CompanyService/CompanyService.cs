using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using IK_Project.Infrastructure.Repositories.Concreates;
using IK_Project.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.CompanyService
{
    public class CompanyService : ICompanyService
    {
        ICompanyRepository _companyRepository;
        ICompanyManagerRepository _companyManagerRepository;
        IDepartmanManagerRepository _departmanManagerRepository;
        IDepartmantRepository _departmantRepository;
        IEmployeeRepository _employeeRepository;
        IPermissionRepository _permissionRepository;
        IAdminRepository _adminRepository;
        IExpenseRepository _expenseRepository;
        IAdvanceRepository _advanceRepository;
        IMapper _mapper;
		//public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
		//{
		//    this._companyRepository = companyRepository;
		//    this._mapper = mapper;

		//}

		public CompanyService(ICompanyRepository companyRepository, ICompanyManagerRepository companyManagerRepository, IDepartmanManagerRepository departmanManagerRepository, IDepartmantRepository departmantRepository, IEmployeeRepository employeeRepository, IPermissionRepository permissionRepository, IAdminRepository adminRepository, IExpenseRepository expenseRepository, IMapper mapper, IAdvanceRepository advanceRepository)
		{
			_companyRepository = companyRepository;
			_companyManagerRepository = companyManagerRepository;
			_departmanManagerRepository = departmanManagerRepository;
			_departmantRepository = departmantRepository;
			_employeeRepository = employeeRepository;
			_permissionRepository = permissionRepository;
			_adminRepository = adminRepository;
			_expenseRepository = expenseRepository;
			_mapper = mapper;
			_advanceRepository = advanceRepository;
		}

		public async Task<IDataResult<List<CompanyListDTO>>> AllCompanies()
        {
            var companies = await _companyRepository.GetAllAsync();
            if (companies.Count() <= 0)
            {
                return new ErrorDataResult<List<CompanyListDTO>>("Sistemde şirket bulunamadı");
            }
            return new SuccessDataResult<List<CompanyListDTO>>(_mapper.Map<List<CompanyListDTO>>(companies), "Şirket Listeleme başarılı");
        }

        public async Task<IDataResult<CompanyCreateDTO>> Create(CompanyCreateDTO companyDTO)
        {
            var iscompany = await _companyRepository.GetAsync(x => x.CompanyName == companyDTO.CompanyName);
            if (iscompany==null)
            {
                var company = _mapper.Map<Company>(companyDTO);
                await _companyRepository.AddAsync(company);
                await _companyRepository.SaveChangeAsync();
                return new SuccessDataResult<CompanyCreateDTO>(_mapper.Map<CompanyCreateDTO>(company), "Ekleme başarılı");
            }
            return new ErrorDataResult<CompanyCreateDTO>("Sistemde şirket zaten kayıtlı.");

        }

        public async Task<IDataResult<CompanyUpdateDTO>> Edit(CompanyUpdateDTO companyDTO)
        {
            var company = await _companyRepository.GetByIdAsync(companyDTO.Id);
            if (company == null)
            {
                return new ErrorDataResult<CompanyUpdateDTO>("Şirket bulunamadı");
            }

            var updatedCompany = _mapper.Map(companyDTO, company);
            await _companyRepository.UpdateAsync(updatedCompany);
            await _companyRepository.SaveChangeAsync();
            return new SuccessDataResult<CompanyUpdateDTO>(_mapper.Map<CompanyUpdateDTO>(updatedCompany), "Güncelleme başarılı");
        }

        public async Task<IDataResult<CompanyDTO>> EditToggle(CompanyDTO companyDTO, bool newstatus)
        {
            var company = await _companyRepository.GetByIdAsync(companyDTO.Id);
            if (company == null)
            {
                return new ErrorDataResult<CompanyDTO>("Company not found");
            }
            company.IsActive = newstatus;

            await _companyRepository.UpdateAsync(company);
            await _companyRepository.SaveChangeAsync();
            return new SuccessDataResult<CompanyDTO>(_mapper.Map<CompanyDTO>(company), "Güncelleme başarılı");
        }

        public async Task<IDataResult<CompanyDTO>> GetById(Guid id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return new ErrorDataResult<CompanyDTO>("Şirket bulunamadı");
            }
            return new SuccessDataResult<CompanyDTO>(_mapper.Map<CompanyDTO>(company), "Şirket başarıyla getirildi");
        }

        public async Task<IDataResult<List<CompanyListDTO>>> GetDefaults(Expression<Func<Company, bool>> expression)
        {
            var companies = await _companyRepository.GetAllAsync();
            if (companies.Count() <= 0)
            {
                return new ErrorDataResult<List<CompanyListDTO>>("Sistemde şirket bulunamadı");
            }
            return new SuccessDataResult<List<CompanyListDTO>>(_mapper.Map<List<CompanyListDTO>>(companies), "Şirket listeleme başarılı");
        }

        public async Task<IResult> Remove(Guid Id)
        {
            var company = await _companyRepository.GetByIdAsync(Id);

            if (company != null)
            {
                company.IsActive = false;

                var departments = await _departmantRepository.GetAllAsync(x => x.CompanyId == company.Id);
                if (departments != null)
                {
                    foreach (var department in departments)
                    {
                        var departmanManager = await _departmanManagerRepository.GetByIdAsyncNull(department.DepartmantManagerId);
                        if (departmanManager != null)
                        {
                            await _departmanManagerRepository.DeleteAsync(departmanManager);
                            var advances = await _advanceRepository.GetAllAsync(x => x.DepartmantManagerId == departmanManager.Id);
                            var expenses = await _expenseRepository.GetAllAsync(x => x.DepartmantManagerId == departmanManager.Id);
                            var permisions = await _permissionRepository.GetAllAsync(x => x.DepartmantManagerId == departmanManager.Id);
                            await _advanceRepository.DeleteRangeAsync(advances);
                            await _expenseRepository.DeleteRangeAsync(expenses);
                            await _permissionRepository.DeleteRangeAsync(permisions);
                            await _departmanManagerRepository.SaveChangeAsync();

                        }

                        var employees = await _employeeRepository.GetAllAsync(x => x.DepartmantId == department.Id);
                        if (employees != null)
                        {
                            foreach (var employee in employees)
                            {

                                var advances = await _advanceRepository.GetAllAsync(x => x.EmployeeId == employee.Id);
                                if (advances != null)
                                {
                                    foreach (var advance in advances)
                                    {
                                        await _advanceRepository.DeleteAsync(advance);
                                        _advanceRepository.SaveChange();
                                    }
                                }

                                var expenses = await _expenseRepository.GetAllAsync(x => x.EmployeeId == employee.Id);
                                if (expenses != null)
                                {
                                    foreach (var expense in expenses)
                                    {
                                        await _expenseRepository.DeleteAsync(expense);
                                        _expenseRepository.SaveChange();
                                    }
                                }

                                var permissions = await _permissionRepository.GetAllAsync(x => x.EmployeeId == employee.Id);
                                if (permissions != null)
                                {
                                    foreach (var permission in permissions)
                                    {
                                        await _permissionRepository.DeleteAsync(permission);
                                        _permissionRepository.SaveChange();
                                    }
                                }
                                await _employeeRepository.DeleteAsync(employee);

                                await _employeeRepository.SaveChangeAsync();

                            }
                        }
                        await _departmantRepository.DeleteAsync(department);
                        await _departmantRepository.SaveChangeAsync();

                    }
                }
                var companyManagers = await _companyManagerRepository.GetByIdAsyncNull(company.CompanyManagerId);
                if (companyManagers != null)
                {
                    await _companyManagerRepository.DeleteAsync(companyManagers);
                }

                await _companyRepository.DeleteAsync(company);
                await _companyRepository.SaveChangeAsync();
                return new SuccessResult("Silme Başarılı");
            }
            return new ErrorResult("silme başarısız.");
        }

        //public async Task<IResult> Remove3(Guid Id)
        //{
        //    var company = await _companyRepository.GetByIdAsync(Id);

        //    if (company != null)
        //    {
        //        company.IsActive = false;

        //        var departments = await _departmantRepository.GetAllAsync(x => x.CompanyId == company.Id);
        //        if (departments != null)
        //        {
        //            foreach (var department in departments)
        //            {
        //                var employees = await _employeeRepository.GetAllAsync(x => x.DepartmantId == department.Id);
        //                if (employees != null)
        //                {
        //                    foreach (var employee in employees)
        //                    {
        //                        var advances = await _advanceRepository.GetAllAsync(x => x.EmployeeId == employee.Id);
        //                        await _advanceRepository.DeleteRangeAsync(advances);

        //                        var expenses = await _expenseRepository.GetAllAsync(x => x.EmployeeId == employee.Id);
        //                        await _expenseRepository.DeleteRangeAsync(expenses);

        //                        var permissions = await _permissionRepository.GetAllAsync(x => x.EmployeeId == employee.Id);
        //                        await _permissionRepository.DeleteRangeAsync(permissions);

        //                        await _employeeRepository.DeleteAsync(employee);
        //                    }
        //                }
        //                await _departmantRepository.DeleteAsync(department);
        //            }
        //        }

        //        var companyManager = await _companyManagerRepository.GetByIdAsyncNull(company.CompanyManagerId);
        //        if (companyManager != null)
        //        {
        //            await _companyManagerRepository.DeleteAsync(companyManager);
        //        }

        //        await _companyRepository.DeleteAsync(company);
        //        await _companyRepository.SaveChangeAsync();

        //        return new SuccessResult("Silme Başarılı");
        //    }

        //    return new ErrorResult("Silme başarısız.");
        //}

        public async Task<IDataResult<CompanyDTO>> GetByIdDepartman(Guid DepartmantId)
        {
            var companies = await _companyRepository.GetByIdAsync(DepartmantId);
            if (companies == null)
            {
                return new ErrorDataResult<CompanyDTO>("Şiket bulunamadı");
            }
            return new SuccessDataResult<CompanyDTO>(_mapper.Map<CompanyDTO>(companies), "Şirketler getirildi");
        }
        public async Task<IDataResult<CompanyDTO>> GetCompany(Expression<Func<Company, bool>> expression)
        {
            var companies = await _companyRepository.GetAsync(expression);
            if (companies == null)
            {
                return new ErrorDataResult<CompanyDTO>("Sistemde şirket bulunamadı");
            }
            return new SuccessDataResult<CompanyDTO>(_mapper.Map<CompanyDTO>(companies), "Şirket listeleme başarılı");
        }
    }
}
