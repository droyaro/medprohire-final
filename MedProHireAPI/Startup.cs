using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using medprohiremvp.DATA.Entity;
using medprohiremvp.DATA.IdentityModels;
using medprohiremvp.Repo.Context;
using medprohiremvp.Repo.IRepository;
using medprohiremvp.Repo.Repository;
using medprohiremvp.Service.EmailServices;
using medprohiremvp.Service.Ilogger;
using medprohiremvp.Service.IServices;
using medprohiremvp.Service.Services;
using medprohiremvp.Service.SignSend;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace MedProHireAPI
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
            services.AddAuthorization();
            services.AddMvc();
            RegisterServices(services);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {
                    Title = "MedProHire API",
                   
                });
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MedProHire API V1");
               
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
          
        }
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<ICommonRepository, CommonRepository>();
            services.AddScoped<ICommonServices, CommonServices>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISignature, Signature>();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEmailLogger, EmailLogger>();
            services.AddTransient<IEmailLoggerProvider, EmailLoggerProvider>();


        }
    }
   
}
