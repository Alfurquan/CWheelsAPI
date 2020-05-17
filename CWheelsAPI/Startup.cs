using AutoMapper;
using CWheelsAPI.Core;
using CWheelsAPI.Extensions;
using CWheelsAPI.Middlewares;
using CWheelsAPI.Models;
using CWheelsAPI.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CWheelsAPI
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

            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            services.AddMvc().AddXmlSerializerFormatters();
            services.AddDbContext<CWheelsDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddAuthenticationServices(Configuration);

            services.AddSwaggerDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,CWheelsDbContext cWheelsDbContext)
        {
            app.UseMiddleware<ExceptionMiddleware>(); 

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
