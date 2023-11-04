﻿using IK_Project.Domain.Enums;

namespace IK_Project.UI.Areas.CompanyManager.Models.CompanyVM
{
    public class CompanyManagerCompanyUpdateVM
    {
        public Guid Id { get; set; }
        public string? CompanyName { get; set; }

        public string? Logo { get; set; }

        public CompanyTitle? CompanyTitle { get; set; }
        public string? MersisNo { get; set; }
        public string? TaxNo { get; set; }
        public string? TaxAdministration { get; set; }
        public IFormFile? LogoFile { get; set; }
        //[NotMapped]
        //[Display(Name = "Logo")]
        //[AllowedExtensions(new string[] { ".jpg", ".png", ".jpeg" })]
        //public IFormFile LogoFile { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int? NumberOfEmployees { get; set; }
        public DateTime? Founded { get; set; }
        public DateTime? DealStartDate { get; set; }
        public DateTime? DealEndDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status Status { get; set; } = Status.Created;
        public Guid? MenuId { get; set; }
        public Guid? CompanyManagerId { get; set; }
        //public int Id { get; set; }
        //public string? CompanyName { get; set; }
        //public string? TaxNo { get; set; }
        //public string? Logo { get; set; }
        //public string? PhoneNumber { get; set; }
        //public string? Address { get; set; }
        //public string? Email { get; set; }
        //public int? NumberOfEmployees { get; set; }
        //public DateTime? DealStartDate { get; set; }
        //public DateTime? DealEndDate { get; set; }
        //public string? CreatedBy { get; set; }
        //public DateTime? CreatedDate { get; set; }
        //public string? ModifiedBy { get; set; }
        //public DateTime? ModifiedDate { get; set; }
        //public Status? Status { get; set; }
        //public int? MenuId { get; set; }
        //public string? MenuName { get; set; }
    }
}
