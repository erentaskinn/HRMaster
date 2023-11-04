using AutoMapper;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.UI.Areas.Admin.Models.ViewModels.AdminVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs;
using IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyVM;
using IK_Project.UI.Areas.Admin.Models.ViewModels.DepartmantVMs;
using IK_Project.UI.Areas.Admin.Models.ViewModels.MenuVMs;
using IK_Project.UI.Models;

namespace IK_Project.UI.MapperProfile.AdminProfile
{
    public class AdminProfile:Profile
    {
        public AdminProfile()
        {
            //CreateMap<AdminListDTO, AdminAdminListVM>();
            CreateMap<AdminAdminCreateVM, AdminCreateDTO>();
            CreateMap<AdminAdminUpdateDTO, AdminAdminUpdateVM>().ReverseMap();
            CreateMap<AdminAdminListVM,AdminListDTO>().ReverseMap();
            CreateMap<AppUserInformationVM, AppUserInformationDTO>();
            CreateMap<IndexCombineViewModel, AppUserInformationDTO>().ReverseMap();
            CreateMap<AppUserListDTO, AppUserInformationListVM>().ReverseMap();
            //CreateMap<AdminAppUserCreateVM, AppUserCreateDTO>();
            //CreateMap<AppUserListDTO, AdminAppUserListVM>();

            // Company

            CreateMap<CompanyCreateDTO, AdminCompanyCreateVM>().ReverseMap();
            CreateMap<CompanyListDTO, AdminCompanyListVM>().ReverseMap();
            CreateMap<CompanyUpdateDTO, AdminCompanyUpdateVM>().ReverseMap();
            CreateMap<CompanyUpdateDTO, AdminCompanyUpdateVM>().ReverseMap();
            CreateMap<CompanyDTO, AdminCompanyUpdateVM>();


            //Departman
            CreateMap<DepartmantListDTO, AdminDepartmantListVM>().ReverseMap();
            CreateMap<DepartmantCreateDTO, AdminDepartmantCreateVM>().ReverseMap();
            CreateMap<DepartmantUpdateDTO, AdminDepartmantUpdateVM>().ReverseMap();

            //Menü
            CreateMap<AdminMenuCreateVM, MenuCreateDTO>().ReverseMap();
            CreateMap<AdminMenuListVM, MenuListDTO>().ReverseMap();
            CreateMap<AdminMenuUpdateVM, MenuUpdateDTO>().ReverseMap();
            CreateMap<MenuDto, AdminMenuUpdateVM>();

            CreateMap<CompanyManagerListDTO, AdminCompanyManagerListVM>();
            CreateMap<AdminCompanyManagerCreateVM, CompanyManagerCreateDTO>().ReverseMap();
            CreateMap<CompanyManagerUpdateDTO, AdminCompanyManagerEditVM>().ReverseMap();

        }
    }
}
