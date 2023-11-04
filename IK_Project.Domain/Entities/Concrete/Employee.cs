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
    public class Employee :BaseUser
    {
       

        public decimal? Salary { get; set; }
        public virtual List<Expense>? Expenses { get; set; }
        public virtual List<Advance>? Advances { get; set; }
        public virtual List<Permission>? Permissions { get; set; }

        public virtual List<Salary>? Salaries { get; set; }

        public virtual Departmant? Departmant { get; set; }

        //Navigation
        public Guid DepartmantId { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
