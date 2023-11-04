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
    public class DepartmantConfiguration:AuditableEntityConfiguration<Departmant>
    {
        public override void Configure(EntityTypeBuilder<Departmant> builder)
        {
            builder.Property(x => x.Name).IsRequired(false).HasMaxLength(35).HasColumnType("varchar");
            base.Configure(builder);
        }
    }
}
