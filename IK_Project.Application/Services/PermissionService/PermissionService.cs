using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using IK_Project.Infrastructure.Repositories.Concreates;
using IK_Project.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace IK_Project.Application.Services.PermissionService
{
    public class PermissionService : IPermissionService
    {
        IPermissionRepository _permissionRepository;
        IMapper _mapper;
        public PermissionService(IPermissionRepository permissionRepository,IMapper mapper)
        {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
        }

		public async Task<IDataResult<List<PermissionListDTO>>> AllPermissions()
		{
			var permissions = await _permissionRepository.GetAllAsync();
			if (permissions.Count() < 0)
			{
				return new ErrorDataResult<List<PermissionListDTO>>("Sistemde Permission bulunamadı.");

			}
			var permissionListDto = _mapper.Map<List<PermissionListDTO>>(permissions);
			return new SuccessDataResult<List<PermissionListDTO>>(permissionListDto, "Permission Listeleme başarılı");
		}

		public async Task<IResult> Create(PermissionCreateDTO permissionCreateDTO)
		{
			var permission = _mapper.Map<Permission>(permissionCreateDTO);
			await _permissionRepository.AddAsync(permission);
			await _permissionRepository.SaveChangeAsync();
			return new SuccessResult("Permission Eklendi");
		}

		public async Task<IResult> Edit(PermissionUpdateDTO permissionUpdateDTO)
		{
			var permission = await _permissionRepository.GetByIdAsync(permissionUpdateDTO.Id);
			if (permission == null)
			{
				return new ErrorResult("Permission bulunamadı");

			}

			var permissionedit = _mapper.Map(permissionUpdateDTO, permission);
			await _permissionRepository.UpdateAsync(permissionedit);
			await _permissionRepository.SaveChangeAsync();
			return new SuccessResult("Permission Güncellendi.");
		}

		public async Task<IDataResult<PermissionUpdateDTO>> GetById(Guid id)
		{
			var permission = await _permissionRepository.GetByIdAsync(id);
			if (permission == null)
			{
				return new ErrorDataResult<PermissionUpdateDTO>("Sistemde Permission bulunamadı.");

			}
			var permissionupdatedto = _mapper.Map<PermissionUpdateDTO>(permission);

			return new SuccessDataResult<PermissionUpdateDTO>(permissionupdatedto, "Belirtilen ID'de Permission var.");
		}

		public async Task<IDataResult<List<PermissionListDTO>>> GetDefaults(Expression<Func<Permission, bool>> expression)
		{
			var permissions = await _permissionRepository.GetDefault(x => x.Status != Status.Deleted);
			if (permissions == null)
			{
				return new ErrorDataResult<List<PermissionListDTO>>("permission listelenemedi.");

			}
			var permissionsdto = _mapper.Map<List<PermissionListDTO>>(permissions);

			return new SuccessDataResult<List<PermissionListDTO>>(permissionsdto, "permission listelendi.");
		}

		public async Task<IResult> Remove(Guid id)
		{
			var permission = await _permissionRepository.GetByIdAsync(id);
			if (permission is null)
			{
				return new ErrorResult("permission Bulunamadi");
			}
			await _permissionRepository.DeleteAsync(permission);
			await _permissionRepository.SaveChangeAsync();
			return new SuccessResult("permission Silme islemi Basarili");
		}

        public async Task<IDataResult<List<PermissionListDTO>>> GetDMAdvance(Guid Id)
        {
            var permission = await _permissionRepository.GetDefault(x => x.DepartmantManagerId == Id && x.Status != Status.Deleted);
            if (permission == null)
            {
                return new ErrorDataResult<List<PermissionListDTO>>("Permission listelenemedi.");
            }
            var permissionListDTOs = _mapper.Map<List<PermissionListDTO>>(permission);

            return new SuccessDataResult<List<PermissionListDTO>>(permissionListDTOs, "permiision listelendi.");
        }

        public async Task<IDataResult<List<PermissionListDTO>>> GetDMPermission(Guid Id)
        {
            var permissions = await _permissionRepository.GetDefault(x => x.DepartmantManagerId == Id && x.Status != Status.Deleted);
            if (permissions == null)
            {
                return new ErrorDataResult<List<PermissionListDTO>>("Permission listelenemedi.");
            }
            var permissionListDTOs = _mapper.Map<List<PermissionListDTO>>(permissions);

            return new SuccessDataResult<List<PermissionListDTO>>(permissionListDTOs, "Permission listelendi.");
        }

        public async Task<IDataResult<List<PermissionListDTO>>> GetEmployeePermission(Guid Id)
        {
            var permissions = await _permissionRepository.GetDefault(x => x.EmployeeId == Id & x.Status != Status.Deleted);
            if (permissions == null)
            {
                return new ErrorDataResult<List<PermissionListDTO>>("Expense listelenemedi.");
            }
            var permissiondto = _mapper.Map<List<PermissionListDTO>>(permissions);

            return new SuccessDataResult<List<PermissionListDTO>>(permissiondto, "Expense listelendi.");
        }
    }
}
