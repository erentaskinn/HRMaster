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
    public class Salary: AuditableEntity
    {
        public int SalaryAmount { get; set; }
        public CurrecyUnit CurrecyUnit { get; set; }
        public int SalaryYear { get; set; }
        public int SalaryMonth { get; set; }
        public Guid? EmployeeID { get; set; }
        public Guid? DepartmantManagerId { get; set; }
        public virtual Employee? Employee { get; set; }

    }
}
