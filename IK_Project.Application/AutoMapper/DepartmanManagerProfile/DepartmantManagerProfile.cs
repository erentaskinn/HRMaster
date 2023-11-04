using AutoMapper;
using IK_Project.Application.Models.DTOs.DepartmantManagerDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.DepartmanManagerProfile
{
    public class DepartmantManagerProfile: Profile
    {
        public DepartmantManagerProfile()
        {
            CreateMap<DepartmantManager, DepartmantManagerListDTO>().ReverseMap();
            CreateMap<DepartmantManager, DepartmantManagerCreateDTO>().ReverseMap();
            CreateMap<DepartmantManager, DepartmantManagerUpdateDTO>().ReverseMap();
            CreateMap<DepartmantManager, DepartmantManagerDTO>().ReverseMap();
        }
    }
}
