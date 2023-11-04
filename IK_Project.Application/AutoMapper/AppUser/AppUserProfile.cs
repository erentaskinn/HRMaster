using AutoMapper;
using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.AppUser
{
    public class AppUserProfile:Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUserInformationDTO, Domain.Entities.Concrete.AppUser>().ReverseMap();
            CreateMap<Domain.Entities.Concrete.AppUser, AppUserInformationDTO>();
            CreateMap<Domain.Entities.Concrete.AppUser, AppUserListDTO>().ReverseMap();
            CreateMap<Domain.Entities.Concrete.AppUser, AppUserUpdateDTO>().ReverseMap();
        }
    }
}
