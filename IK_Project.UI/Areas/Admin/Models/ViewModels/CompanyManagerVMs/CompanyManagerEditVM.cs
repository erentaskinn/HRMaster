using IK_Project.Application.Models.DTOs.AppUserDTOs;
using IK_Project.Application.Models.DTOs.CompanyDTOs;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace IK_Project.UI.Areas.Admin.Models.ViewModels.CompanyManagerVMs
{
    public class CompanyManagerEditVM
    {
        public Guid Id { get; set; }
        public string? ProfilePhoto { get; set; }
        [Required(ErrorMessage = "*Name is required.")]
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        [Required(ErrorMessage = "*LastName is required.")]
        public string? LastName { get; set; }
        public string? SecondLastName { get; set; }
        [Required(ErrorMessage = "*BirthDate is required.")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "*Birht Place is required.")]
        public string? BirhtPlace { get; set; }
        public string? CitizenId { get; set; }
        public DateTime StartDate { get; set; }
        public Profession? Profession { get; set; }
        public Department? Department { get; set; }
        [EmailAddress(ErrorMessage = "Please enter a valid e-mail address.")]
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid AppUserID { get; set; }
        public int CompanyID { get; set; }
        public List<UserListDTO>? AppUserDTOs { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status? Status { get; set; }
    }
}
