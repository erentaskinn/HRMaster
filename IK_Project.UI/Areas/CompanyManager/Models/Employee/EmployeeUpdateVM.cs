using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Application.Models.DTOs.EmployeeDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace IK_Project.UI.Areas.CompanyManager.Models.Employee
{
    public class EmployeeUpdateVM
    {
        public Guid Id { get; set; }
        public string? ProfilePhoto { get; set; }
        [Required(ErrorMessage = "*Name is required.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "*LastName is required.")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "*BirthDate is required.")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "*Birht Place is required.")]
        public string? BirhtPlace { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string? Email { get; set; }
        public Guid AppUserID { get; set; }
        public int CompanyID { get; set; }

        public List<CompanyListDTO>? CompanyDTOs { get; set; }
        public Company? Company { get; set; }
        public AppUser? AppUser { get; set; }
        public string? userPassword { get; set; }
        public string? userName { get; set; }
        public Status Status => Status.Created;
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string? CustomEmail { get; set; }
        public List<EmployeeListDTO>? EmployeeDTOs { get; set; }
        [Required(ErrorMessage = "*Start Date is required.")]
        public List<DepartmantListDTO>? DepartmantDTOs { get; set; }
        public int? DepartmantId { get; set; }
        public string DepartmantName { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
