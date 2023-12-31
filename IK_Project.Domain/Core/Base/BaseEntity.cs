﻿
using IK_Project.Domain.Core.Interfaces;
using IK_Project.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Core.Base
{
	public abstract class BaseEntity : IUpdatetableEntity
	{

		public string? ModifiedBy { get; set; }
		public DateTime? ModifiedDate { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.Now;
		public Guid Id { get; set; }
		public Status Status { get; set; } = Status.Created;
	}
}
