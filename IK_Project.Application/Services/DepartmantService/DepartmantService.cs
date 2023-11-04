using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
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

namespace IK_Project.Application.Services.DepartmantService
{
    public class DepartmantService : IDepartmantService
    {
        private readonly IDepartmantRepository _departmantRepository;
        private readonly IMapper _mapper;
        public DepartmantService(IDepartmantRepository departmantRepository, IMapper mapper)
        {
            _departmantRepository = departmantRepository;
            _mapper = mapper;
        }
        public async Task<IDataResult<List<DepartmantListDTO>>> AllDepartmants()
        {
            var departmants = await _departmantRepository.GetAllAsync();
            if (departmants.Count() <= 0)
            {
                return new ErrorDataResult<List<DepartmantListDTO>>("Sistemde departmant bulunamadı");
            }
            return new SuccessDataResult<List<DepartmantListDTO>>(_mapper.Map<List<DepartmantListDTO>>(departmants), "departmant Listeleme başarılı");
        }
        public async Task<IDataResult<DepartmantCreateDTO>> Create(DepartmantCreateDTO departmantDto)
        {
            var departmant = _mapper.Map<Departmant>(departmantDto);
            await _departmantRepository.AddAsync(departmant);
            await _departmantRepository.SaveChangeAsync();
            return new SuccessDataResult<DepartmantCreateDTO>(_mapper.Map<DepartmantCreateDTO>(departmant), "Ekleme başarılı");
        }
        public async Task<IDataResult<DepartmantUpdateDTO>> Edit(DepartmantUpdateDTO departmantDto)
        {
            var departmant = await _departmantRepository.GetByIdAsync(departmantDto.Id);
            if (departmant == null)
            {
                return new ErrorDataResult<DepartmantUpdateDTO>("Paket bulunamadı");
            }
            var updatedDepartmant = _mapper.Map(departmantDto, departmant);
            await _departmantRepository.UpdateAsync(updatedDepartmant);
            await _departmantRepository.SaveChangeAsync();
            return new SuccessDataResult<DepartmantUpdateDTO>(_mapper.Map<DepartmantUpdateDTO>(updatedDepartmant), "Güncelleme başarılı");
        }

        public async Task<IDataResult<DepartmantDTO>> GetById(Guid id)
        {
            var departmant = await _departmantRepository.GetByIdAsync(id);
            if (departmant == null)
            {
                return new ErrorDataResult<DepartmantDTO>("departmant bulunamadı");
            }
            return new SuccessDataResult<DepartmantDTO>(_mapper.Map<DepartmantDTO>(departmant), "departmant başarıyla getirildi");
        }
        public async Task<IDataResult<List<DepartmantListDTO>>> GetDefaults(Expression<Func<Departmant, bool>> expression)
        {
            var departmant = await _departmantRepository.GetAllAsync(expression);
            if (departmant.Count() <= 0)
            {
                return new ErrorDataResult<List<DepartmantListDTO>>("Sistemde departmant bulunamadı");
            }
            return new SuccessDataResult<List<DepartmantListDTO>>(_mapper.Map<List<DepartmantListDTO>>(departmant), "Departmant listeleme başarılı");
        }
        public async Task<IResult> Remove(Guid id)
        {
            var departmant = await _departmantRepository.GetByIdAsync(id);
            if (departmant == null)
            {
                return new ErrorResult("Silinecek departmant bulunamadı");
            }
            await _departmantRepository.DeleteAsync(departmant);
            await _departmantRepository.SaveChangeAsync();
            return new SuccessResult("Silme Başarılı");
        }
    }
}
