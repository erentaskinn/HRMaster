using AutoMapper;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Application.Models.DTOs.ExpenseDTOs;
using IK_Project.Application.Models.DTOs.PermissionDTOs;
using IK_Project.Domain.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.AutoMapper.EmployeeProfile
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeDTO,Employee >().ReverseMap();

            CreateMap<Company, CompanyDTO>();

            CreateMap<Departmant,DepartmantDTO>().ReverseMap();

            CreateMap<Expense, ExpenseUpdateDTO>().ReverseMap();

            CreateMap<Permission, PermissionCreateDTO>();
         
        }
    }
}
