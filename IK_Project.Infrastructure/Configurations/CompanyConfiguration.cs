using IK_Project.Domain.Core.EntityTypeConfiguration;
using IK_Project.Domain.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Configurations
{
    public class CompanyConfiguration : AuditableEntityConfiguration<Company>
	{
		public override void Configure(EntityTypeBuilder<Company> builder)
		{

			builder.Property(x => x.CompanyName).HasMaxLength(128).IsRequired();
			builder.Property(x => x.MersisNo).HasMaxLength(128).IsRequired(false);
			builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
			builder.Property(x => x.NumberOfEmployees).IsRequired();
			builder.Property(x => x.DealStartDate).HasColumnType("date").IsRequired();
			builder.Property(x => x.DealEndDate).HasColumnType("date").IsRequired();
			builder.Property(x => x.Founded).HasColumnType("date").IsRequired(false);
			builder.Property(x => x.TaxNo).IsRequired();
			builder.Property(x => x.TaxAdministration).IsRequired(false);
			builder.Property(x => x.Logo).IsRequired(false);
			builder.Property(x => x.Address).IsRequired();
			builder.Property(x => x.PhoneNumber).IsRequired();
			builder.Property(x => x.IsActive).HasDefaultValue(true);


            base.Configure(builder);

		}

	}
}
