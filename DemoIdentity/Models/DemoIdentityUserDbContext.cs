using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoIdentity.Models
{
    public class DemoIdentityUserDbContext : IdentityDbContext<DemoIdentityUser>
    {
        public DemoIdentityUserDbContext(DbContextOptions<DemoIdentityUserDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DemoIdentityUser>(user => user.HasIndex(x => x.Locale).IsUnique(false));
            builder.Entity<Organization>(org =>
            {
                org.ToTable("Organizations");
                org.HasKey(x =>x.Id);
                org.HasMany<DemoIdentityUser>().WithOne().HasForeignKey(x => x.OrgId).IsRequired(false);
            });
        }
    }
}
