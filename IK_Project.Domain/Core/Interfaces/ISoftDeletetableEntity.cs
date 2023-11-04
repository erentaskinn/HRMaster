using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Core.Interfaces
{
    public interface ISoftDeletetableEntity: IEntity, ICreatetableEntity, IUpdatetableEntity
    {
        string? DeletedBy { get; set; }
        DateTime? DeletedDate { get; set; }
    }
}
