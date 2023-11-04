using IK_Project.Domain.Core.EntityTypeConfiguration;
using IK_Project.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Configurations
{
	public class SalaryConfiguration : AuditableEntityConfiguration<Salary>
	{
		public override void Configure(EntityTypeBuilder<Salary> builder)
		{
			builder.Property(x => x.SalaryAmount).IsRequired();
			builder.Property(x => x.CurrecyUnit).IsRequired();
			builder.Property(x => x.SalaryYear).IsRequired();
			//builder.Property(x => x.SalaryMonth).IsRequired(false);
			base.Configure(builder);
		}
	}
}
