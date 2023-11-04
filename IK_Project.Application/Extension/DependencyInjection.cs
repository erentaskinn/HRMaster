using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IK_Project.Application.Services.CompanyService;
using IK_Project.Application.Services.AdminSevice;
using IK_Project.Application.Services.EmployeeService;
using IK_Project.Application.Services.CompanyManagerService;
using IK_Project.Application.Services.MenuService;
using IK_Project.Application.Services.AppRoleService;
using IK_Project.Application.Services.ExpenseService;
using IK_Project.Application.Services.PermissionService;
using IK_Project.Application.Services.AdvanceService;
using IK_Project.Application.Services.DepartmantService;
using Microsoft.EntityFrameworkCore.SqlServer.ValueGeneration.Internal;
using IK_Project.Application.Services.AccountService;
using Castle.Core.Smtp;
using IK_Project.Application.Services.EmailSenderService;
using IK_Project.Application.Services.DepartmantManagerService;
using IK_Project.Application.Services.AppUserService;

namespace IK_Project.Application.Extension
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationService(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddScoped<ICompanyService, CompanyService>();
			services.AddScoped<IAdminService, AdminService>();
			services.AddScoped<IEmployeeService, EmployeeService>();
			services.AddScoped<ICompanyManagerService, CompanyManagerService>();
			services.AddScoped<IMenuService, MenuService>();
			services.AddScoped<IExpenseService, ExpenseService>();
			services.AddScoped<IPermissionService, PermissionService>();
			services.AddScoped<IAdvanceService, AdvanceService>();
			services.AddScoped<IDepartmantService, DepartmantService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<IEMailSenderService, EmailSenderService>();
			services.AddScoped<IDepartmantManagerService,DepartmantManagerService>();
			services.AddScoped<IAppUserService, AppUserService>();

			return services;
		}
	}
}
