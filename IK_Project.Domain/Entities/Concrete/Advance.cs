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
    public class Advance : AuditableEntity
    {
       
        
        public ConfirmationStatus ConfirmationStatus { get; set; } = ConfirmationStatus.Pending;
        public string? Description { get; set; }
        public int? Amount { get; set; }
        public DateTime? DateOfReply { get; set; }
        public CurrecyUnit CurrencyUnit { get; set; }
        public AdvanceType AdvanceType { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? DepartmantManagerId { get; set; }
        public Guid AppUserId { get; set; }


        //public virtual Employee? Employee { get; set; }





    }
}
