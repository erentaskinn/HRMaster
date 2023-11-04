using IK_Project.Domain.Enums;

namespace IK_Project.UI.Areas.CompanyManager.Models.PermissionVM
{
    public class CompanyManagerPermissionUpdateVM
    {
        public Guid Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status? Status { get; set; }

        public string? EmployeeName { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; }
        public string? Description { get; set; }
        public int? Amount { get; set; }
        public DateTime? DateOfReply { get; set; }
        public CurrecyUnit CurrencyUnit { get; set; }
        public AdvanceType? AdvanceType { get; set; }

        public Guid? EmployeeId { get; set; }
        public Guid? DepartmantManagerId { get; set; }
    }
}
