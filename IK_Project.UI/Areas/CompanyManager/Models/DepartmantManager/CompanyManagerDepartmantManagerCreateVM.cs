using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace IK_Project.UI.Areas.CompanyManager.Models.DepartmanManager
{
    public class CompanyManagerDepartmantManagerCreateVM
    {
        //public Guid Id { get; set; }

        public IFormFile? LogoFile { get; set; }

        public string? Name { get; set; }
        //public string? SecondName { get; set; }
        public string? LastName { get; set; }
        //public string? SecondLastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirhtPlace { get; set; }
        //[Required(ErrorMessage = "*CitizenId is required.")]
        //public string? CitizenId { get; set; }
        //[Required(ErrorMessage = "*Start Date is required.")]
        public DateTime? StartDate { get; set; }
        //[Required(ErrorMessage = "*Profession is required.")]
        //public Profession? Profession { get; set; }
        //[Required(ErrorMessage = "*Department is required.")]
        //public Department? Department { get; set; }
        //[Required(ErrorMessage = "*Email is required.")]

        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid AppUserID { get; set; }
        public Guid? CompanyID { get; set; }
        //public List<CompanyListDTO>? CompanyDTOs { get; set; }


        //public Company? Company { get; set; }
        public AppUser? AppUser { get; set; }
        //[Required(ErrorMessage = "*User Password is required.")]
        public string? Password { get; set; }
        //[Required(ErrorMessage = "*UserName is required.")]

        public string? userName { get; set; }

        public Status Status => Status.Created;
        public string? CustomEmail { get; set; }
        public SelectList? CompanyList { get; set; }
        public bool PasswordChangeRequired { get; set; }
        public bool IsActive { get; set; } = true;
        public string? DepartmantName { get; set; }
        public List<DepartmantListDTO>? DepartmantDTOs { get; set; }
        public Guid? DepartmantId { get; set; }
        public string CompanyName { get; set; }
    }
}
