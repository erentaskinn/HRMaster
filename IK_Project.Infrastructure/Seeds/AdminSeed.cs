using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IK_Project.Infrastructure.DataAccess;
using IK_Project.Domain.Enums;

namespace IK_Project.Infrastructure.Seeds
{
    public static class AdminSeed
    {

        private const string adminEmail = "admin@hrmaster.web.tr";
        private const string adminPassword = "BilgeAdamHS-10";

        public static async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder<IKProjectDBContext>(); 
            dbContextBuilder.UseSqlServer(configuration.GetConnectionString("dbConnection"));

            using IKProjectDBContext context = new IKProjectDBContext(dbContextBuilder.Options);
            if (!context.Roles.Any()) 
            {
                await AddRoles(context);

            }
            if (!context.Users.Any(user=>user.Email==adminEmail)) 
            {
                await AddAdmin(context);
            }
            await Task.CompletedTask;  // İşlem tamamlandı


        }
        private static async Task AddAdmin(IKProjectDBContext context)
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpperInvariant(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpperInvariant(),
                EmailConfirmed = true,
                

            };
            user.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(user, adminPassword);
            await context.Users.AddAsync(user);
            var adminRoleId =  context.Roles.FirstOrDefault(x => x.Name == Roles.Admin.ToString())!.Id; //! null olmadığını garanti eder.
            await context.UserRoles.AddAsync(new IdentityUserRole<string> 
            {
                RoleId = adminRoleId,
                UserId = user.Id
                
            });

            await context.SaveChangesAsync();
        }
        private static async Task AddRoles(IKProjectDBContext context)
        {
            string[] roles = Enum.GetNames(typeof(Roles));
            foreach (string item in roles)
            {

 
                if (await context.Roles.AnyAsync(role=> role.Name == item))
                {
                    continue;
                }
                IdentityRole role = new IdentityRole();
                {
                    role.Name = item;
                    role.NormalizedName = item.ToUpperInvariant();
                }
                await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();
            }
        
        }

         

    }
}
