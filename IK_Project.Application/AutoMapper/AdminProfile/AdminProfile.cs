using AutoMapper;
using IK_Project.Application.Models.DTOs.AdminDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Application.Models.DTOs.MenuDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.AdminProfile
{
    public class AdminProfile: Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminListDTO>();
            CreateMap<AdminCreateDTO, Admin>();
            CreateMap<Admin, AdminListDTO>();



            CreateMap<Menu, MenuListDTO>();
            CreateMap<Menu, MenuDto>();


            CreateMap<Company, CompanyUpdateDTO>().ReverseMap();
            CreateMap<Company, CompanyListDTO>().ForMember(dest => dest.CompanyManagerName, config => config.MapFrom(x => x.CompanyManager.Name));
            CreateMap<Company, CompanyCreateDTO>().ReverseMap();
            CreateMap<Company, CompanyDTO>();


            CreateMap<CompanyManager, CompanyManagerListDTO>().ForMember(dest => dest.CompanyName, config => config.MapFrom(x => x.Company.CompanyName));
            CreateMap<CompanyManager, CompanyManagerCreateDTO>().ReverseMap();




        }
    }
}
