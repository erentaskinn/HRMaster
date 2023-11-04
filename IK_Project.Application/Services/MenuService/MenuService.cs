using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
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

namespace IK_Project.Application.Services.MenuService
{
    public class MenuService : IMenuService
    {
        public MenuService(IMenuRepository menuRepository,IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        IMenuRepository _menuRepository;
        IMapper _mapper;
        public async Task<IDataResult<List<MenuListDTO>>> AllMenus()
        {
            var paket = await _menuRepository.GetAllAsync();
            if (paket.Count() <= 0)
            {
                return new ErrorDataResult<List<MenuListDTO>>("Sistemde paket bulunamadı");
            }
            return new SuccessDataResult<List<MenuListDTO>>(_mapper.Map<List<MenuListDTO>>(paket), "paket Listeleme başarılı");
        }

        public async Task<IDataResult<MenuCreateDTO>> Create(MenuCreateDTO menuDTO)
        {
            var paket = _mapper.Map<Menu>(menuDTO);
            await _menuRepository.AddAsync(paket);
            await _menuRepository.SaveChangeAsync();
            return new SuccessDataResult<MenuCreateDTO>(_mapper.Map<MenuCreateDTO>(paket), "Ekleme başarılı");
        }

        public async Task<IDataResult<MenuUpdateDTO>> Edit(MenuUpdateDTO menuDTO)
        {
            var paket = await _menuRepository.GetByIdAsync(menuDTO.Id);
            if (paket == null)
            {
                return new ErrorDataResult<MenuUpdateDTO>("Paket bulunamadı");
            }
            //var hasBook = await _menuRepository.AnyAsync(x => x.Id == paket.Id);
            //if (hasBook)
            //{
            //    return new ErrorDataResult<MenuUpdateDTO>("Kitap zaten var");
            //}
            var updatedBook = _mapper.Map(menuDTO, paket);
            await _menuRepository.UpdateAsync(updatedBook);
            await _menuRepository.SaveChangeAsync();
            return new SuccessDataResult<MenuUpdateDTO>(_mapper.Map<MenuUpdateDTO>(updatedBook), "Güncelleme başarılı");
        }
        public async Task<IDataResult<MenuDto>> EditToggle(MenuDto menuDTO, bool newStatus)
        {
            var menu = await _menuRepository.GetByIdAsync(menuDTO.Id);
            if (menu == null)
            {
                return new ErrorDataResult<MenuDto>("Package not found");
            }
            menu.IsActive = newStatus;
            await _menuRepository.UpdateAsync(menu);
            await _menuRepository.SaveChangeAsync();
            return new SuccessDataResult<MenuDto>(_mapper.Map<MenuDto>(menu), "Güncelleme başarılı");

        }


        public async Task<IDataResult<List<MenuListDTO>>> GetDefaults(Expression<Func<Menu, bool>> expression)
        {
            var paket = await _menuRepository.GetAllAsync();
            if (paket.Count() <= 0)
            {
                return new ErrorDataResult<List<MenuListDTO>>("Sistemde paket bulunamadı");
            }
            return new SuccessDataResult<List<MenuListDTO>>(_mapper.Map<List<MenuListDTO>>(paket), "paket Listeleme başarılı");
        }

        //public async Task<bool> IsMenuExsist(string menuName)
        //{
        //    return await _menuRepository.Any(x => x.Name.Contains(menuName));
        //}

        public async Task<IResult> Remove(Guid id)
        {
            var paket = await _menuRepository.GetByIdAsync(id);
            if (paket == null)
            {
                return new ErrorResult("Silinecek Paket bulunamadı");
            }
            await _menuRepository.DeleteAsync(paket);
            await _menuRepository.SaveChangeAsync();
            return new SuccessResult("Silme Başarılı");
        }

        public async Task<IDataResult<MenuDto>> GetById(Guid id)
        {
            var paket = await _menuRepository.GetByIdAsync(id);
            if (paket == null)
            {
                return new ErrorDataResult<MenuDto>("paket bulunamadı");
            }
            return new SuccessDataResult<MenuDto>(_mapper.Map<MenuDto>(paket), "paket başarıyla getirildi");
        }
        //public async Task Edit2(MenuUpdateDTO menuUpdateDTO)
        //{
        //    var menu = _mapper.Map<Menu>(menuUpdateDTO);
        //    await _menuRepository.Update2(menu);
        //}
    }
}
