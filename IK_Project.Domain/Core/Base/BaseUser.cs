using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Core.Base
{
    public class BaseUser: AuditableEntity
    {

		public string? IdentityId { get; set; }

		public string? ProfilePhoto { get; set; }
		public string? Name { get; set; }
		public string? LastName { get; set; }
		public DateTime? BirthDate { get; set; }
		public string? BirhtPlace { get; set; }
		//public string? CitizenId { get; set; }
		public DateTime? StartDate { get; set; }
        //public Profession? Profession { get; set; }
        //public Departmant? Departmant { get; set; }
        //public Department? Department { get; set; }
        public bool PasswordChangeRequired { get; set; } = false;
        public string Email { get; set; }
		public string? Address { get; set; }
		public string? PhoneNumber { get; set; }

	}
}
