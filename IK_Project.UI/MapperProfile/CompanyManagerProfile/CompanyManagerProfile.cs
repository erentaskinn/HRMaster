using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.UI.Areas.CompanyManager.Models.AdvanceVM;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyManager;
using IK_Project.UI.Areas.CompanyManager.Models.CompanyVM;
using IK_Project.UI.Areas.CompanyManager.Models.DepartmanManager;
using IK_Project.UI.Areas.CompanyManager.Models.Departmant;
using IK_Project.UI.Areas.CompanyManager.Models.Employee;
using IK_Project.UI.Areas.CompanyManager.Models.ExpenseVM;
using IK_Project.UI.Areas.CompanyManager.Models.PermissionVM;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerAdvanceVM;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerExpenseVMs;
using IK_Project.UI.Areas.DepartmantManager.Models.DepartmantManagerPermission;
using IK_Project.UI.Areas.Employee.Models.AdvanceVMs;
using IK_Project.UI.Areas.Employee.Models.ExpenseVMs;
using IK_Project.UI.Areas.Employee.Models.PermissionVMs;

namespace IK_Project.UI.MapperProfile.CompanyManagerProfile
{
    public class CompanyManagerProfile:Profile
    {
        public CompanyManagerProfile()
        {
            CreateMap<CompanyManagerUpdateDTO, CompanyManagerCompanyManagerUpdateVM>().ReverseMap();

            CreateMap<CompanyUpdateDTO, CompanyManagerCompanyUpdateVM>().ReverseMap();

            CreateMap<CompanyDTO, CompanyManagerCompanyUpdateVM>().ReverseMap();

            CreateMap<DepartmantCreateDTO, CompanyManagerDepartmantCreateVM>().ReverseMap();
            CreateMap<DepartmantListDTO, CompanyManagerDepartmantListVM>().ReverseMap();
            CreateMap<DepartmantUpdateDTO, CompanyManagerDepartmantUpdateVM>().ReverseMap();
            CreateMap<DepartmantDTO, CompanyManagerDepartmantUpdateVM>().ReverseMap();

            CreateMap<EmployeeListDTO, CompanyManagerEmployeeListVM>().ReverseMap();
            CreateMap<EmployeeCreateDTO, CompanyManagerEmployeeCreateVM>().ReverseMap();
            CreateMap<EmployeeUpdateDTO, CompanyManagerEmployeeUpdateVM>().ReverseMap();
            CreateMap<EmployeeDTO, CompanyManagerEmployeeUpdateVM>().ReverseMap();

            CreateMap<DepartmantManagerListDTO, CompanyManagerDepartmantManagerListVM>().ReverseMap();
            CreateMap<DepartmantManagerCreateDTO, CompanyManagerDepartmantManagerCreateVM>().ReverseMap();
            CreateMap<DepartmantManagerUpdateDTO, CompanyManagerDepartmantManagerUpdateVM>().ReverseMap();
            CreateMap<DepartmantManagerDTO, CompanyManagerDepartmantManagerUpdateVM>().ReverseMap();

            CreateMap<CompanyManagerPermissionListVM, PermissionListDTO>().ReverseMap();
            CreateMap<CompanyManagerExpenseListVM, ExpenseListDTO>().ReverseMap();
            CreateMap<CompanyManagerAdvanceListVM, AdvanceListDTO>().ReverseMap();

            CreateMap<CompanyManagerPermissionUpdateVM, PermissionListDTO>().ReverseMap();
            CreateMap<CompanyManagerExpenseUpdateVM, ExpenseListDTO>().ReverseMap();
            CreateMap<CompanyManagerAdvanceUpdateVM, AdvanceListDTO>().ReverseMap();

            CreateMap<ExpenseCreateVM, ExpenseCreateDTO>().ReverseMap();
            CreateMap<ExpenseListVM, ExpenseListDTO > ().ReverseMap();
            CreateMap<ExpenseUpdateVM, ExpenseUpdateDTO>().ReverseMap();

            CreateMap<PermissionCreateVM, PermissionCreateDTO>().ReverseMap();
            CreateMap<PermissionListVM, PermissionListDTO > ().ReverseMap();
            CreateMap<PermissionUpdateVM, PermissionUpdateDTO>().ReverseMap();

            CreateMap<AdvanceCreateVM, AdvanceCreateDTO>().ReverseMap();
            CreateMap<AdvanceListDTO, AdvanceListVM> ().ReverseMap();
            CreateMap<AdvanceUpdateVM, AdvanceUpdateDTO>().ReverseMap();
        }
    }
}
