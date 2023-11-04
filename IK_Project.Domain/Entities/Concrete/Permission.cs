using IK_Project.Domain.Core.Base;
using IK_Project.Domain.Entities.Abstract;
using IK_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Entities.Concrete
{
    public class Permission : AuditableEntity
    {
        public PermissionType PermissionType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public DateTime? DateOfReply { get; set; }
        public string? PermissionFilePath { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; } 
        public string? ReasonOfRejection { get; set; }

        public Guid? EmployeeId { get; set; }
        public Guid? DepartmantManagerId { get; set; }
        public Guid AppUserId { get; set; }

        

        //public virtual Employee? Employee { get; set; }
    }
}
