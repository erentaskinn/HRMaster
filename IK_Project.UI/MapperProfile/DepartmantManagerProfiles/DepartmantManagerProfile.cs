using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.UI.Areas.DepartmantManager.Models;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerAdvanceVM;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerExpenseVMs;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerPermission;

namespace IK_Project.UI.MapperProfile.DepartmantManagerProfiles
{
    public class DepartmantManagerProfile:Profile
    {
        public DepartmantManagerProfile()
        {
            CreateMap<DepartmantManagerUpdateVM, DepartmantManagerUpdateDTO>().ReverseMap();
            CreateMap<DepartmanManagerPermissionListVM,PermissionListDTO>().ReverseMap();
            CreateMap<DepartmantManagerExpenseListVM, ExpenseListDTO>().ReverseMap();
            CreateMap<DepartmantManagerAdvanceListVM, AdvanceListDTO>().ReverseMap();
            CreateMap<DepartmantManagerAdvanceCreatVm,AdvanceCreateDTO>().ReverseMap();
            CreateMap<DepartmantManagerExpenseCreateVM, ExpenseCreateDTO>().ReverseMap();
            CreateMap<DepartmantManagerPermissionCreateVM, PermissionCreateDTO>().ReverseMap();
            CreateMap<DepartmantManagerAdvanceUpdateVM,AdvanceUpdateDTO>().ReverseMap();
            CreateMap<DepartmantManagerAdvanceListVM,AdvanceListDTO>().ReverseMap();
            CreateMap<DepartmantManagerExpenseUpdateVm,ExpenseUpdateDTO>().ReverseMap();
            CreateMap<DepartmantManagerPermissionUpdateVM, PermissionUpdateDTO>().ReverseMap();
        }
    }
}
