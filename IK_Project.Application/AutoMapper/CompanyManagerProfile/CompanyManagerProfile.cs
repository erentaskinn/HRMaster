using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyManagerDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.CompanyManagerProfile
{
    public class CompanyManagerProfile: Profile
    {
        public CompanyManagerProfile()
        {
            CreateMap<CompanyManager, CompanyManagerCreateDTO>().ReverseMap();
            CreateMap<CompanyManager, CompanyManagerListDTO>().ReverseMap();
            CreateMap<CompanyManager, CompanyManagerUpdateDTO>().ReverseMap();
        }
    }
}
