using System;
using aspnetdemo2.Areas.Identity.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(aspnetdemo2.Areas.Identity.IdentityHostingStartup))]
namespace aspnetdemo2.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<aspnetdemo2IdentityDbContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("aspnetdemo2IdentityDbContextConnection")));

                services.AddDefaultIdentity<IdentityUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                
                }
                )
                .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<aspnetdemo2IdentityDbContext>();
                

                services.AddAuthentication()
                    .AddGoogle(options => 
                    {
                        options.ClientId = "ABCEDKDJflkjsdlafj";
                        options.ClientSecret = "Supper dupper secret";
                    })
                    .AddFacebook(facebookOptions =>
{
    facebookOptions.AppId = "FACEBOOKID";
    facebookOptions.AppSecret = "FECABOOKSECRET";
});;

            });
        }
    }
}