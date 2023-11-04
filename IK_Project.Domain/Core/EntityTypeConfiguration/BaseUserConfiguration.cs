
using IK_Project.Domain.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Domain.Core.EntityTypeConfiguration
{
    public class BaseUserConfiguration<T> : AuditableEntityConfiguration<T> where T : BaseUser
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Name).HasMaxLength(128).IsRequired(false);
            builder.Property(x => x.LastName).HasMaxLength(128).IsRequired(false);
            builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
            builder.Property(x => x.BirthDate).HasColumnType("date").IsRequired(false);
            builder.Property(x => x.StartDate).HasColumnType("date").IsRequired(false);
            builder.Property(x => x.ProfilePhoto).IsRequired(false);
            builder.Property(x => x.IdentityId).IsRequired(false);
            builder.Property(x => x.BirhtPlace).IsRequired(false);
            builder.Property(x => x.Address).IsRequired(false);
            builder.Property(x => x.PhoneNumber).IsRequired(false);
        }
    }
}
