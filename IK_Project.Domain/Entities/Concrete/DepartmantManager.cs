using IK_Project.Domain.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Entities.Concrete
{
    public class DepartmantManager:BaseUser
    {
        //public virtual Departmant? Departmant{ get; set; }
        public Guid? EmployeeId  { get; set; }
        public bool IsActive { get; set; } = true;
        //public Guid? DepartmantId { get; set; }
    }
}
