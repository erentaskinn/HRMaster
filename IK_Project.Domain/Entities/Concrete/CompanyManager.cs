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
    public  class CompanyManager :BaseUser
    {
        public virtual Company? Company { get; set; }
        public bool IsActive { get; set; } = true;

        public Guid? CompanyId { get; set; }

    }
}
