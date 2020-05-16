using System;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Revoow.Data;

[assembly: HostingStartup(typeof(Revoow.Areas.Identity.IdentityHostingStartup))]
namespace Revoow.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {

                services.AddTransient<IEmailSender, EmailSender>();
                services.AddDbContext<AppDbContext>(options => options
                        .UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));
                services.AddIdentity<RevoowUser, IdentityRole>(
                    options =>
                    {
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                    })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders()
                    .AddRoles<IdentityRole>();

                services.AddScoped<IUserClaimsPrincipalFactory<RevoowUser>, RevoowUserClaimsPrincipalFactory>();

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("IsRevoowUser", policy => policy.RequireRole("RevoowUser"));
                    options.AddPolicy("IsStarter", policy => policy.RequireClaim("AccountType", AccountType.Starter.ToString()));
                    options.AddPolicy("IsSmall", policy => policy.RequireClaim("AccountType", AccountType.Small.ToString()));
                    options.AddPolicy("IsProfessional", policy => policy.RequireClaim("AccountType", AccountType.Professional.ToString()));
                });
            });

        }
    }
}