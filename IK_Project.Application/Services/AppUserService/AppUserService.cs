using AutoMapper;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using IK_Project.Infrastructure.Repositories.Interfaces;
using System.Linq.Expressions;

namespace IK_Project.Application.Services.AppUserService
{
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _appUserpository;
        private readonly IMapper _mapper;

        public AppUserService(IAppUserRepository appUserpository, IMapper mapper)
        {
            _appUserpository = appUserpository;
            _mapper = mapper;
        }

        public async Task<IDataResult<AppUserInformationDTO>> CreateAsync(AppUserInformationDTO appUserInformationDTO)
        {
            var appUserInformation = _mapper.Map<AppUser>(appUserInformationDTO);
            await _appUserpository.AddAsync(appUserInformation);
            await _appUserpository.SaveChangeAsync();
            var result = new SuccessDataResult<AppUserInformationDTO>(_mapper.Map<AppUserInformationDTO>(appUserInformation), "Information message addition is successful.");
            return result;
        }
        public async Task<IDataResult<List<AppUserListDTO>>> AllAppUserInformation(Expression<Func<AppUser, bool>> expression)
        {
            var appUserInformation = await _appUserpository.GetAllAsync(expression);
            if (appUserInformation.Count() <= 0)
            {
                return new ErrorDataResult<List<AppUserListDTO>>("Information User not found.");
            }
            return new SuccessDataResult<List<AppUserListDTO>>(_mapper.Map<List<AppUserListDTO>>(appUserInformation), "Information User listed successfully.");
        }
        public async Task<IDataResult<AppUserUpdateDTO>> GetById(Guid id)
        {
            var appUser = await _appUserpository.GetByIdAsync(id);
            if (appUser == null)
            {
                return new ErrorDataResult<AppUserUpdateDTO>("Sistemde AppUser Informaiton bulunamadı.");

            }
            var appUserupdatedto = _mapper.Map<AppUserUpdateDTO>(appUser);

            return new SuccessDataResult<AppUserUpdateDTO>(appUserupdatedto, "Belirtilen ID'de appUserInnformation var.");
        }
        public async Task<IDataResult<AppUserUpdateDTO>> Edit(AppUserUpdateDTO appUserDTO)
        {
            var appUser = await _appUserpository.GetByIdAsync(appUserDTO.Id);
            if (appUser == null)
            {
                return new ErrorDataResult<AppUserUpdateDTO>( "appuser bulunamadı");
            }
            var updatedappUser = _mapper.Map(appUserDTO, appUser);
            await _appUserpository.UpdateAsync(updatedappUser);
            await _appUserpository.SaveChangeAsync();
            return new SuccessDataResult<AppUserUpdateDTO>(_mapper.Map<AppUserUpdateDTO>(updatedappUser), "Güncelleme başarılı");
        }
    }
}
