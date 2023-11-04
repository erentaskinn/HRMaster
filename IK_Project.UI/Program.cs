using IK_Project.Domain.Entities.Concrete;
using IK_Project.Infrastructure.DataAccess;
using IK_Project.Infrastructure.Extension;
using IK_Project.Infrastructure.Repositories.Concreates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using IK_Project.Application.Extension;
using System;
using IK_Project.UI.Extension;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSeedDataService(builder.Configuration); // migration atmadan önce bunu yorum yap. 

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



//builder.Services.ConfigureApplicationCookie(options =>
//{
//    // Cookie settings
//    options.Cookie.HttpOnly = true;
//    options.ExpireTimeSpan = TimeSpan.FromSeconds(12000);

//    options.LoginPath = "/Account/Login";
//    //options.AccessDeniedPath = "//Account/AccessDenied";
//    options.SlidingExpiration = true;
//});

//builder.Services.AddTransient<IEmailSender, EmailSenderService>();
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddMvcService();



//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IKProjectDBContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseAuthentication();
app.UseCookiePolicy();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
//name: "areas",
//pattern: "{area:admin}/{controller=Admin}/{action=Create}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();










//app.MapDefaultControllerRoute();




