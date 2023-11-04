using IK_Project.Domain.Core.EntityTypeConfiguration;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Configurations
{
	public class AdvanceConfiguration : AuditableEntityConfiguration<Advance>
	{
		public override void Configure(EntityTypeBuilder<Advance> builder)
		{
			builder.Property(x => x.Description).IsRequired(false);
			builder.Property(x => x.Amount).IsRequired(false);
			builder.Property(x => x.CurrencyUnit).IsRequired();
			builder.Property(x => x.AdvanceType).IsRequired();
			//builder.Property(x => x.ConfirmationStatus).IsRequired().HasDefaultValue(Status.Pending);
			builder.Property(x => x.DateOfReply).HasColumnType("date").IsRequired(false);
			base.Configure(builder);
		}
	}
}
