using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Revoow.Config;
using Revoow.Data.IRepositories;
using Revoow.Data.Repositories;
using Revoow.Services;
using Revoow.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.UI.Services;
using Revoow.Areas.Identity;
using Stripe;
using Revoow.Options;
using System.Diagnostics;

namespace Revoow
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddControllersWithViews(o => o.Filters.Add(new AuthorizeFilter()));
            services.AddRazorPages();

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie();
            
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<ITestimonialRepository, TestimonialRepository>();
            services.AddTransient<VideoService>();
            services.AddTransient<PaymentService>();
            var autoMapper = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile()));
            services.AddSingleton(autoMapper.CreateMapper());

            services.AddOptions();
            services.Configure<StripeOptions>(Configuration.GetSection("Stripe"));
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "reviewPage",
                    pattern: "{pageUrl}/{action=Detail}",
                    defaults: new { controller = "Page" });
                endpoints.MapRazorPages();
            });

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["ApiKey"];

        }
    }
}
