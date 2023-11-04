
using IK_Project.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Core.Base
{
	public abstract class AuditableEntity : BaseEntity, ISoftDeletetableEntity
	{

		public string? DeletedBy { get; set; }
		public DateTime? DeletedDate { get; set; }
	}
}
