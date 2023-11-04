using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Models.DTOs.DepartmantDTOs;
using IK_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace IK_Project.UI.Areas.CompanyManager.Models.DepartmanManager
{
    public class CompanyManagerDepartmantManagerUpdateVM
    {
        public Guid Id { get; set; }
        public IFormFile LogoFile { get; set; }
        public string? ProfilePhoto { get; set; }

        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirhtPlace { get; set; }
        public string? CitizenId { get; set; }
        public DateTime StartDate { get; set; }
        public Profession? Profession { get; set; }
        public Department? Department { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid AppUserID { get; set; }
        public Guid CompanyID { get; set; }
        public List<UserListDTO>? AppUserDTOs { get; set; }
        public List<DepartmantListDTO>? DepartmantDTOs { get; set; }
        public Guid? DepartmantId { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status? Status { get; set; }
        public bool IsActive { get; set; }
        public string? DepartmantName { get; set; }
    }
}
