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
    public class Menu : AuditableEntity
    {
        public string? Name { get; set; }
        public int? Period { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public int? UserAmount { get; set; }
        public CurrecyUnit? Currecy { get; set; }
        //navigation property
        public virtual ICollection<Company> Companies { get; set; }
        public bool? IsActive { get; set; } 

    }
}
