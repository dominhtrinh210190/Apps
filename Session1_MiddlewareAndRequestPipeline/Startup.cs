using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Session1_MiddlewareAndRequestPipeline.Extensions;
using Session1_MiddlewareAndRequestPipeline.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1_MiddlewareAndRequestPipeline
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
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) => {
                await context.Response.WriteAsync("middleware 0 \n");
                await next.Invoke();
                await context.Response.WriteAsync("middleware quay nguoc lai \n");
            });

            // call middleware theo cách truyền vào trực tiếp
            app.UseMiddleware<CustomMiddleware>();

            // call theo cách truyền vào phương thức mở rộng 
            app.UseMiddlewareExten();

            app.Run(async (context) => {
                await context.Response.WriteAsync("middleware 1 \n");
            });
        }
    }
}
