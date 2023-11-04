using AutoMapper;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.DepartmantManagerProfiles
{
    public class DepartmantMnagerProfile:Profile
    {
        public DepartmantMnagerProfile()
        {
            CreateMap<DepartmantManager, DepartmantCreateDTO>().ReverseMap();
            CreateMap<DepartmantManager, DepartmantManagerListDTO>().ReverseMap();
            CreateMap<DepartmantManager, DepartmantManagerUpdateDTO>().ReverseMap();
        }
       
    }
}
