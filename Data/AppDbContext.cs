using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Revoow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Revoow.Areas.Identity;

namespace Revoow.Data
{
    public class AppDbContext : IdentityDbContext<RevoowUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options, 
                            IHttpContextAccessor httpContextAccessor
                            ) : base(options)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; }

        public override int SaveChanges()
        {
            var httpContext = this.HttpContextAccessor.HttpContext;
            var userName = httpContext.User.Identity.Name;
            var user = this.Users.First<RevoowUser>(r => r.UserName == userName);

            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseModel && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).TimeModified = DateTime.Now;
                ((BaseModel)entityEntry.Entity).ModifiedBy = user;

                if (entityEntry.State == EntityState.Modified)
                {
                    entityEntry.Reference("CreatedBy").IsModified = false;
                    entityEntry.Property("TimeCreated").IsModified = false;
                }

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).TimeCreated = DateTime.Now;
                    ((BaseModel)entityEntry.Entity).CreatedBy = user;
                }
            }

            return base.SaveChanges();
        }
    }
}