using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Repo.Context;
using medprohiremvp.Repo.IRepository;
using medprohiremvp.Repo.Repository;
using medprohiremvp.Service.IServices;
using medprohiremvp.Service.Services;
using medprohiremvp.Service.SignSend;
using medprohiremvp.Service.EmailServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using medprohiremvp.DATA.Entity;

namespace CMSmedprohiremvp
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
            services.Configure<RootPath>(Configuration.GetSection("RootPath"));
            services.AddDbContext<MvpDBContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<AppUserContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();
           

            services.AddAuthentication()
               .AddCookie(options =>
               {
                   options.LoginPath = "/Login/";
                   options.AccessDeniedPath = "/Login/";
               });

            services.AddIdentity<ApplicationUser, ApplicationRole>(
               options =>
               {
                   options.User.RequireUniqueEmail = true;
                   options.Password.RequireUppercase = true;
                   options.Password.RequireDigit = true;
                   options.Password.RequireNonAlphanumeric = true;
               }
           )
          .AddEntityFrameworkStores<AppUserContext>()
          .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Login/";
                options.AccessDeniedPath = "/Login/";
                options.SlidingExpiration = true;
            });
            RegisterServices(services);
            services.AddAuthorization();
        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

            }

            app.UseStaticFiles();
     

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = "/StaticFiles"
            //});
            app.UseAuthentication();

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                  Path.Combine(env.ContentRootPath, "node_modules")
              ),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=index}/{id?}");
            });
        }
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<ICommonServices, CommonServices>();
            services.AddScoped<ISignature, Signature>();
            services.AddScoped<IEmailService, EmailService>();

        }
    }
}
