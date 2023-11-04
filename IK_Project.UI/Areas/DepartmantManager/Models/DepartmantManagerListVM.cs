using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;

namespace IK_Project.UI.Areas.DepartmantManager.Models
{
    public class DepartmantManagerListVM
    {
        public Guid Id { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Status Status { get; set; } = Status.Created;
        public string? IdentityId { get; set; }
        public string? ProfilePhoto { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirhtPlace { get; set; }
        public DateTime? StartDate { get; set; }
        public bool PasswordChangeRequired { get; set; } = false;
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public virtual Departmant Departmant { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
