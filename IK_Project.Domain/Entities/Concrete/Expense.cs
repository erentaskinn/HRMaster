using IK_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using IK_Project.Domain.Entities.Abstract;
using IK_Project.Domain.Core.Base;

namespace IK_Project.Domain.Entities.Concrete
{
    public class Expense: AuditableEntity
    {
       
        public string? ExpenseFilePath { get; set; }
        public int? Amount { get; set; }
        public DateTime? DateOfReply { get; set; } = DateTime.Now;
        public CurrecyUnit CurrencyUnit { get; set; }
        public string? ReasonOfRejection { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public ConfirmationStatus ConfirmationStatus { get; set; } 

        public Guid? EmployeeId { get; set; }
        public Guid? DepartmantManagerId { get; set; }
        public Guid AppUserId { get; set; }

        //public virtual Employee? Employee { get; set; }


    }
}
