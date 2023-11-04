using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.UI.Areas.Employee.Models;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using IK_Project.UI.Areas.Employee.Models.PermissionVMs;

namespace IK_Project.UI.MapperProfile.EmployeeProfile
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeVM, EmployeeDTO>().ReverseMap();
            CreateMap<EmployeeVM, EmployeeUpdateDTO>().ReverseMap();

            CreateMap<AdvanceListVM, AdvanceListDTO>().ReverseMap();
            CreateMap<AdvanceUpdateDTO, AdvanceUpdateVM>();

            CreateMap<ExpenseListDTO,ExpenseListVM>();
            CreateMap<ExpenseCreateVM,ExpenseCreateDTO>().ReverseMap();
            CreateMap<ExpenseDTO, ExpenseCreateVM>().ReverseMap();
            CreateMap<ExpenseUpdateDTO,ExpenseUpdateVM>().ReverseMap();

            CreateMap<PermissionListDTO, PermissionListVM>();
            CreateMap<PermissionCreateDTO, PermissionCreateVM>();
            CreateMap<PermissionUpdateDTO, PermissionUpdateVM>().ReverseMap();

        }
    }
}
