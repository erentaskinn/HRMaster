﻿using IK_Project.Domain.Enums;

namespace IK_Project.UI.Areas.CompanyManager.Models.CompanyManager
{
    public class CompanyManagerCompanyManagerUpdateVM
    {
        public Guid? Id { get; set; }
        public IFormFile? LogoFile { get; set; }
        public string? ProfilePhoto { get; set; }
        public string Name { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string? SecondLastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirhtPlace { get; set; }
        public string? CitizenId { get; set; }
        public DateTime? StartDate { get; set; }
        public Profession? Profession { get; set; }
        public Department? Department { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? AppUserID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Status? Status { get; set; }
    }
}