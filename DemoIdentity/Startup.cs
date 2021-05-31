using DemoIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DemoIdentity
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
            var connectionString = @"Data Source=(LocalDb)\MSSQLLocalDB;database=IdentityDemo.DemoIdentityUser;trusted_connection=yes;";
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<DemoIdentityUserDbContext>(opt => opt.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly)));
            services.AddIdentity<DemoIdentityUser, IdentityRole>(options =>
            {
                //options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "emailconf";
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 4;
                options.User.RequireUniqueEmail = true;

                //options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
                .AddEntityFrameworkStores<DemoIdentityUserDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<EmailConfirmationTokenProvider<DemoIdentityUser>>("emailconf")
                .AddPasswordValidator<DoesNotContainPasswordValidator<DemoIdentityUser>>();
            services.AddScoped<IUserClaimsPrincipalFactory<DemoIdentityUser>, DemoIdentityUserClaimsPrincipalFactory>();

            services.AddScoped<IUserStore<DemoIdentityUser>, UserOnlyStore<DemoIdentityUser, DemoIdentityUserDbContext>>();

            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromHours(3));
            services.Configure<EmailConfirmationTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(2));
            services.ConfigureApplicationCookie(options => options.LoginPath = "/Home/Login");

            services.AddAuthentication().AddGoogle("google", options => 
            {
                options.ClientId = "809112919860-h1n7dc7dirso4lvvl6saqqduui0ajdno.apps.googleusercontent.com";
                options.ClientSecret = "5emMid3h8mhXcHfcXH2IlfFP";
                options.SignInScheme=IdentityConstants.ExternalScheme;
            });


            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
