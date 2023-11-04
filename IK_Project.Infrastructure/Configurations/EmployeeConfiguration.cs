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
    public class EmployeeConfiguration: BaseUserConfiguration<Employee>
    {
		public override void Configure(EntityTypeBuilder<Employee> builder)
		{
			builder.Property(x => x.Salary).IsRequired(false);
            builder.Property(x => x.IsActive).HasDefaultValue(true);


            base.Configure(builder);
		}
	}
}
