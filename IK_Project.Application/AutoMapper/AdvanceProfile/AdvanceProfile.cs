using AutoMapper;
using IK_Project.Application.Models.DTOs.AdvanceDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.AdvanceProfile
{
    public class AdvanceProfile : Profile
    {
        public AdvanceProfile()
        {
            CreateMap<Advance, AdvanceCreateDTO>().ReverseMap();
            CreateMap<Advance, AdvanceListDTO>().ReverseMap();
            CreateMap<Advance, AdvanceUpdateDTO>().ReverseMap();
        }
    }
}
