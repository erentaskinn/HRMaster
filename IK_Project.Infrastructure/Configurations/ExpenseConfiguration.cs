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
	public class ExpenseConfiguration : AuditableEntityConfiguration<Expense>
	{
		public override void Configure(EntityTypeBuilder<Expense> builder)
		{
			builder.Property(x => x.ExpenseFilePath).IsRequired(false);
			builder.Property(x => x.Amount).IsRequired(false);
			builder.Property(x => x.CurrencyUnit).IsRequired();
			builder.Property(x => x.ReasonOfRejection).IsRequired(false);
			builder.Property(x => x.ExpenseType).IsRequired();
			//builder.Property(x => x.ConfirmationStatus).IsRequired().HasDefaultValue("Pending");
			builder.Property(x => x.DateOfReply).HasColumnType("date").IsRequired();
			base.Configure(builder);
		}
	}
}
