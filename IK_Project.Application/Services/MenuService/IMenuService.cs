using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Utilities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Services.MenuService
{
    public interface IMenuService
    {

        Task<IDataResult<MenuCreateDTO>> Create(MenuCreateDTO menuDTO);
        Task<IDataResult<MenuUpdateDTO>> Edit(MenuUpdateDTO menuDTO);
        Task<IResult> Remove(Guid id);
        Task<IDataResult<List<MenuListDTO>>> GetDefaults(Expression<Func<Menu, bool>> expression);
        Task<IDataResult<List<MenuListDTO>>> AllMenus();
        Task<IDataResult<MenuDto>> GetById(Guid id);
        //Task<bool> IsMenuExsist(string menuName);
        Task<IDataResult<MenuDto>> EditToggle(MenuDto menuDTO, bool newStatus);

    }
}
