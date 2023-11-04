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
    public class Departmant : AuditableEntity
    {

        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? DepartmantManagerId { get; set; }
        //navigation property
        public virtual Company? Company { get; set; }
        public virtual DepartmantManager? DepartmantManager { get; set; }
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
