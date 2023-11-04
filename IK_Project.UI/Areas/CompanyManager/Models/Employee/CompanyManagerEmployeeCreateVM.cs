using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace IK_Project.UI.Areas.CompanyManager.Models.Employee
{
    public class CompanyManagerEmployeeCreateVM
    {
        public Guid Id { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }

        public string? BirhtPlace { get; set; }

        public string? Email { get; set; }
        public Guid? AppUserID { get; set; }
        public Guid? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public List<CompanyListDTO>? CompanyDTOs { get; set; }
        public Company? Company { get; set; }
        public AppUser? AppUser { get; set; }
        public string? Password { get; set; }
        public string? userName { get; set; }
        public Status Status => Status.Created;
        public string? CustomEmail { get; set; }
        public List<EmployeeListDTO>? EmployeeDTOs { get; set; }
        public List<DepartmantListDTO>? DepartmantDTOs { get; set; }
        public Guid? DepartmantId { get; set; }
        public DateTime? StartDate { get; set; }
        public bool? PasswordChangeRequired { get; set; }
    }
}
