using IK_Project.Domain.Core.Base;
using IK_Project.Domain.Core.EntityTypeConfiguration;
using IK_Project.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Configurations
{
	public class PermissionConfiguration: AuditableEntityConfiguration<Permission>
	{
		public override void Configure(EntityTypeBuilder<Permission> builder)
		{
			builder.Property(x => x.PermissionType).IsRequired();
			builder.Property(x => x.NumberOfDays).IsRequired();
			builder.Property(x => x.PermissionFilePath).IsRequired(false);
			builder.Property(x => x.ReasonOfRejection).IsRequired(false);
			//builder.Property(x => x.ConfirmationStatus).IsRequired().HasDefaultValue("Pending");
			builder.Property(x => x.StartDate).HasColumnType("date").IsRequired();
			builder.Property(x => x.EndDate).HasColumnType("date").IsRequired();
			builder.Property(x => x.DateOfReply).HasColumnType("date").IsRequired(false);
			base.Configure(builder);
		}
	}
}
