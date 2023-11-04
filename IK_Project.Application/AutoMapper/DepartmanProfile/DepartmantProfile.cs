using AutoMapper;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.DepartmanProfile
{
    public class DepartmantProfile: Profile
    {
        public DepartmantProfile()
        {
            CreateMap<Departmant, DepartmantCreateDTO>().ReverseMap();
            CreateMap<Departmant, DepartmantUpdateDTO>().ReverseMap();
            CreateMap<Departmant, DepartmantDTO>().ReverseMap();
            CreateMap<Departmant, DepartmantDTO>().ReverseMap();
        }
    }
}
