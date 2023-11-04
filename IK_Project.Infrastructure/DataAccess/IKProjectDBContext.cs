using IK_Project.Domain.Entities.Abstract;
using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using IK_Project.Domain.Core.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IK_Project.Infrastructure.DataAccess
{
    public class IKProjectDBContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IKProjectDBContext(DbContextOptions<IKProjectDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IKProjectDBContext(DbContextOptions<IKProjectDBContext> options) : base(options)
        {

        }

        public DbSet<CompanyManager> CompanyManagers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Advance> Advances { get; set; }
        public DbSet<Departmant> Departmants { get; set; }
        public DbSet<DepartmantManager> DepartmantManagers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
            modelBuilder.Entity<Company>()
.HasOne(c => c.CompanyManager)
.WithOne(cm => cm.Company)
.HasForeignKey<CompanyManager>(cm => cm.CompanyId);
//            modelBuilder.Entity<Departmant>()
//.HasOne(c => c.DepartmantManager)
//.WithOne(cm => cm.Departmant)
//.HasForeignKey<DepartmantManager>(cm => cm.DepartmantId);
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            SetBaseProperties();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseProperties();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetBaseProperties()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var userId = _httpContextAccessor?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user bulunamadı";

            foreach (var entry in entries)
            {
                SetIfAdded(entry, userId);
                SetIfModified(entry, userId);
                SetIfDeleted(entry, userId);
            }
        }

        private void SetIfDeleted(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State != EntityState.Deleted)
            {
                return;
            }

            if (entry.Entity is not AuditableEntity entity)
            {
                return;
            }

            entry.State = EntityState.Modified;
            entity.DeletedDate = DateTime.Now;
            entity.Status = Domain.Enums.Status.Deleted;
            entity.DeletedBy = userId;
        }

        private void SetIfModified(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Status = Domain.Enums.Status.Modified;
                entry.Entity.ModifiedBy = userId;
                entry.Entity.ModifiedDate = DateTime.Now;
            }
        }

        private void SetIfAdded(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Status = Domain.Enums.Status.Created;
                entry.Entity.CreatedDate = DateTime.Now;
                entry.Entity.CreatedBy = userId;
            }
        }

    }




}
