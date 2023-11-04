using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess;
using IK_Project.Infrastructure.Repositories.Concreates;
using IK_Project.Infrastructure.Repositories.Interfaces;
using IK_Project.Infrastructure.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Infrastructure.Extension
{
	public static class DependesyInjection
	{
		public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<IKProjectDBContext>(options => {
				options.UseLazyLoadingProxies();
				options.UseSqlServer(configuration.GetConnectionString("dbConnection"));
			});



			services.AddScoped<ICompanyRepository, CompanyRepository>();
			services.AddScoped<IAdminRepository, AdminRepository>();
			services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			services.AddScoped<ICompanyManagerRepository, CompanyManagerRepository>();
			services.AddScoped<IDepartmantRepository, DepartmantRepository>();
			services.AddScoped<IExpenseRepository, ExpenseRepository>();
			services.AddScoped<IPermissionRepository, PermissionRepository>();
			services.AddScoped<IAdvanceRepository, AdvanceRepository>();
			services.AddScoped<IMenuRepository, MenuRepository>();
			services.AddScoped<IDepartmanManagerRepository,DepartmantManagerRepository>();
			services.AddScoped<IAppUserRepository, AppUserRepository>();
			//services.AddIdentity().AddEntityFrameworkStores<>(IKProjectDBContext);
			return services;

		}

		public static IServiceCollection AddSeedDataService(this IServiceCollection services, IConfiguration configuration)
		{
			AdminSeed.SeedAsync(configuration).GetAwaiter().GetResult(); //getawaiter: asenkron olmasına rağmen cevabı bekle senkron gibi davran
			return services;
		}
	}
}
