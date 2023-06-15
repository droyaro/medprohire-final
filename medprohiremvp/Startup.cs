using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Repo.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using medprohiremvp.Repo.IRepository;
using medprohiremvp.Repo.Repository;
using medprohiremvp.Service.IServices;
using medprohiremvp.Service.Services;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Service.SignSend;
using medprohiremvp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using medprohiremvp.DATA.Entity;
using medprohiremvp.Service.Ilogger;
using Microsoft.Extensions.Logging;

namespace medprohiremvp
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
           
            services.Configure<TimespanModel>(Configuration.GetSection("Timespan"));
            services.Configure<RootPath>(Configuration.GetSection("RootPath"));
            services.AddDbContext<MvpDBContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AppUserContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                  .AddCookie(options =>
                  {
                      options.LoginPath = "/Home/Index";
                      options.AccessDeniedPath = "/Home/Login";
                      options.Cookie.Name = "Medprohire";
                      options.Cookie.Expiration = TimeSpan.FromMinutes(60);
                      options.Cookie.HttpOnly = true;
                      options.ExpireTimeSpan = TimeSpan.FromDays(3);
                      options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                      
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
                options.LoginPath = "/Home/Login";
                options.AccessDeniedPath = "/Home/Login";
                options.Cookie.Name = "Medprohire";
                options.Cookie.Expiration = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(3);
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
                
            });

            RegisterServices(services);
            services.AddMvc();
            services.AddAuthorization();
            services.AddSession(options =>
            {
                options.Cookie.Name = ".medprohiremvp.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
            });
        
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IEmailLoggerProvider emailLoggerProvider)
        {
           
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                loggerFactory.AddProvider(emailLoggerProvider);
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");

            }
            else
            {
                loggerFactory.AddProvider(emailLoggerProvider);
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }

         
            app.UseDefaultFiles();
            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();

            //publish error
            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "node_modules")
                ),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });
            //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
            //{
            //    HotModuleReplacement = true
            //});
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<ICommonServices, CommonServices>();
            services.AddScoped<IAppliedShiftRepository, AppliedShiftRepository>();
            services.AddScoped<IAppliedShiftServices, AppliedShiftSerivces>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignature, Signature>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailLogger, EmailLogger>();
            services.AddTransient<IEmailLoggerProvider, EmailLoggerProvider>();


        }
    }
}
