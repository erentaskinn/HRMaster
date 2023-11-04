using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
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

namespace IK_Project.Application.Services.AdvanceService
{
    public class AdvanceService : IAdvanceService
    {
        IAdvanceRepository _advanceRepository;
        IMapper _mapper;
        public AdvanceService(IAdvanceRepository advanceRepository, IMapper mapper)
        {
			_advanceRepository = advanceRepository;
            _mapper = mapper;
        }

		public async Task<IDataResult<List<AdvanceListDTO>>> AllAdvances()
		{
			var advances = await _advanceRepository.GetAllAsync();
			if (advances.Count() < 0)
			{
				return new ErrorDataResult<List<AdvanceListDTO>>("Sistemde Advance bulunamadı.");

			}
			var advancesListDto = _mapper.Map<List<AdvanceListDTO>>(advances);
			return new SuccessDataResult<List<AdvanceListDTO>>(advancesListDto, "Advance Listeleme başarılı");


		}

		public async Task<IResult> Create(AdvanceCreateDTO advanceCreateDTO)
		{

			var advance = _mapper.Map<Advance>(advanceCreateDTO);
			await _advanceRepository.AddAsync(advance);
			await _advanceRepository.SaveChangeAsync();
			return new SuccessResult("Advance Eklendi");
		}

		public async Task<IResult> Edit(AdvanceUpdateDTO advanceUpdateDTO)
		{
			var advance = await _advanceRepository.GetByIdAsync(advanceUpdateDTO.Id);
			if (advance == null)
			{
				return new ErrorResult("Advance bulunamadı");

			}

			var advanceedit = _mapper.Map(advanceUpdateDTO, advance);
			await _advanceRepository.UpdateAsync(advanceedit);
			await _advanceRepository.SaveChangeAsync();
			return new SuccessResult("Advance Güncellendi.");
		}

		public async Task<IDataResult<AdvanceUpdateDTO>> GetById(Guid id)
		{
			var advance = await _advanceRepository.GetByIdAsync(id);
			if (advance == null)
			{
				return new ErrorDataResult<AdvanceUpdateDTO>("Sistemde Advance bulunamadı.");

			}
			var advanceupdatedto = _mapper.Map<AdvanceUpdateDTO>(advance);

			return new SuccessDataResult<AdvanceUpdateDTO>(advanceupdatedto, "Belirtilen ID'de Advance var.");
		}

		public async Task<IDataResult<List<AdvanceListDTO>>> GetDefaults(Expression<Func<Advance, bool>> expression)
		{
			var advances = await _advanceRepository.GetDefault(x => x.Status != Status.Deleted);
			if (advances == null)
			{
				return new ErrorDataResult<List<AdvanceListDTO>>("Advances listelenemedi.");

			}
			var advancesdto = _mapper.Map<List<AdvanceListDTO>>(advances);

			return new SuccessDataResult<List<AdvanceListDTO>>(advancesdto, "Advances listelendi.");
		}

        public async Task<IDataResult<List<AdvanceListDTO>>> GetDMAdvance(Guid Id)
        {
			var advance = await _advanceRepository.GetDefault(x => x.DepartmantManagerId == Id && x.Status != Status.Deleted);
			if (advance==null)
			{
                return new ErrorDataResult<List<AdvanceListDTO>>("Advances listelenemedi.");
            }
            var advancesdto = _mapper.Map<List<AdvanceListDTO>>(advance);

            return new SuccessDataResult<List<AdvanceListDTO>>(advancesdto, "Advances listelendi.");
        }

        public async Task<IDataResult<List<AdvanceListDTO>>> GetEmployeeAdvance(Guid Id)
        {
			var advance = await _advanceRepository.GetDefault(x => x.EmployeeId == Id & x.Status != Status.Deleted);
			if (advance == null)
			{
				return new ErrorDataResult<List<AdvanceListDTO>>("Advances listelenemedi.");
            }
            var advancesdto = _mapper.Map<List<AdvanceListDTO>>(advance);

            return new SuccessDataResult<List<AdvanceListDTO>>(advancesdto, "Advances listelendi.");
        }

        public async Task<IResult> Remove(Guid id)
		{
			var advance = await _advanceRepository.GetByIdAsync(id);
			if (advance is null)
			{
				return new ErrorResult("Advance Bulunamadi");
			}
			await _advanceRepository.DeleteAsync(advance);
			await _advanceRepository.SaveChangeAsync();
			return new SuccessResult("Advance Silme islemi Basarili");
		}




























		//public async Task<List<AdvanceListDTO>> AllAdvances()
		//{
		//    return _mapper.Map<List<AdvanceListDTO>>(await _advanceRepository.GetAll());
		//}

		//public async Task Create(AdvanceCreateDTO advanceCreateDTO)
		//{
		//    var advance = _mapper.Map<Advance>(advanceCreateDTO);
		//    await _advanceRepository.Add(advance);
		//}

		//public async Task Edit(AdvanceUpdateDTO advanceUpdateDTO)
		//{
		//    var advance = _mapper.Map<Advance>(advanceUpdateDTO);
		//    await _advanceRepository.Update(advance);
		//}

		//public async Task<AdvanceUpdateDTO> GetById(int id)
		//{
		//    return _mapper.Map<AdvanceUpdateDTO>(await _advanceRepository.GetBy(x => x.Id == id));
		//}

		//public async Task<List<AdvanceListDTO>> GetDefaults(Expression<Func<Advance, bool>> expression)
		//{
		//    var result = await _advanceRepository.GetDefault(expression);
		//    var listAdvanceResult = _mapper.Map<List<Advance>, List<AdvanceListDTO>>(result);
		//    return listAdvanceResult;

		//}



		//public async Task Remove(int id)
		//{
		//    Advance advance = await _advanceRepository.GetBy(x => x.Id == id);
		//    await _advanceRepository.Delete(advance);
		//}
	}
}
