﻿using IK_Project.Domain.Enums;

namespace IK_Project.UI.Areas.CompanyManager.Models.ExpenseVM
{
    public class CompanyManagerExpenseListVM
    {
        public string? EmployeeName { get; set; }
        public Guid Id { get; set; }
        public string ExpenseFilePath { get; set; }
        public int Amount { get; set; }
        public DateTime? DateOfReply { get; set; }
        public CurrecyUnit CurrencyUnit { get; set; }
        public string ReasonOfRejection { get; set; }

        public ExpenseType ExpenseType { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; }

        public Guid EmployeeId { get; set; }
        public Guid AppUserId { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status? Status { get; set; }
        public IK_Project.Domain.Entities.Concrete.Employee? Employee { get; set; }
        //public Guid Id { get; set; }
        //public string? CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string? ModifiedBy { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public Status? Status { get; set; }

        //public ConfirmationStatus ConfirmationStatus { get; set; }
        //public string? Description { get; set; }
        //public int? Amount { get; set; }
        //public DateTime? DateOfReply { get; set; }
        //public CurrecyUnit CurrencyUnit { get; set; }
        //public AdvanceType? AdvanceType { get; set; }

        //public Guid? EmployeeId { get; set; }
        //public Guid? DepartmantManagerId { get; set; }
    }
}
