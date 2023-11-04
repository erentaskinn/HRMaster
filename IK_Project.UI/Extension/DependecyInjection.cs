using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System;
using IK_Project.Infrastructure.DataAccess;
using FluentValidation.AspNetCore;
using FluentValidation;

namespace IK_Project.UI.Extension
{
	public static class DependecyInjection
	{
		public static IServiceCollection AddMvcService(this IServiceCollection services)
		{
			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			//services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			services.AddIdentityService();
			services.AddControllersWithViews(opt => opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
            services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddNotyf(config =>
            //{
            //    config.HasRippleEffect = true;
            //    config.DurationInSeconds = 5;
            //    config.Position = NotyfPosition.BottomRight;
            //    config.IsDismissable = true;
            //});
            return services;
		}
		private static IServiceCollection AddIdentityService(this IServiceCollection services)
		{
            services.AddIdentity<IdentityUser, IdentityRole>(x =>
            {
                x.SignIn.RequireConfirmedEmail = false;
                x.SignIn.RequireConfirmedPhoneNumber = false;
                x.SignIn.RequireConfirmedAccount = false;
                x.Password.RequiredLength = 8;
                x.Password.RequireNonAlphanumeric = false;
                x.Password.RequireUppercase = false;
                x.Password.RequireLowercase = false;
                x.Password.RequireDigit = true;
                x.Password.RequiredUniqueChars = 0;


            }).AddEntityFrameworkStores<IKProjectDBContext>().AddDefaultTokenProviders().AddTokenProvider<EmailTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider);
            // Şifre sıfırlama token ayarları
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2); // Token'ın geçerlilik süresi 2 saat
                                                               // Diğer token ayarları...
            });

           
            //services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IKProjectDBContext>();
            return services;
		}
	}
}
