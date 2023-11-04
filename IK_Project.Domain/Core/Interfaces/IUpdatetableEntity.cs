using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Core.Interfaces
{
    public interface IUpdatetableEntity : IEntity, ICreatetableEntity
    {
		public string? ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
	}
}
