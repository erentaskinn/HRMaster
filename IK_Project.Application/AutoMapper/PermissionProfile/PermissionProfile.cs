using AutoMapper;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.PermissionProfile
{
    public class PermissionProfile:Profile
    {
        public PermissionProfile()
        {
            CreateMap<Permission, PermissionCreateDTO>().ReverseMap();
            CreateMap<Permission, PermissionListDTO>().ReverseMap();
            CreateMap<Permission, PermissionUpdateDTO>().ReverseMap();
        }
    }
}
