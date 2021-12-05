using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
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

        public string GetJson()
        {
            var productjson = new
            {
                name = "IPhone 11",
                price = 1000
            };
            return JsonConvert.SerializeObject(productjson);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();                          // chỉ định chúng ta sử dụng design pattenr MVC
            services.AddDistributedMemoryCache();       // Thêm dịch vụ dùng bộ nhớ lưu cache (session sử dụng dịch vụ này)
            services.AddSession();                      // Thêm  dịch vụ Session, dịch vụ này cunng cấp Middleware: 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Thêm StaticFileMiddleware - nếu Request là yêu cầu truy cập file tĩnh,
            // Nó trả ngay về Response nội dung file và là điểm cuối pipeline, nếu  khác
            // nó gọi  Middleware phía sau trong Pipeline
            app.UseStaticFiles();

            // Thêm SessionMiddleware:  khôi phục, thiết lập - tạo ra session
            // gán context.Session, sau đó chuyển gọi ngay middleware
            // tiếp trong pipeline
            app.UseSession();

            // Thêm EndpointRoutingMiddleware: ánh xạ Request gọi đến Endpoint (Middleware cuối)
            // phù hợp định nghĩa bởi EndpointMiddleware
            app.UseRouting();

            // app.UseEndpoint dùng để xây dựng các endpoint - điểm cuối  của pipeline theo Url truy cập
            // Dùng để map các url với các middleware này 
            app.UseEndpoints(endpoints =>
            { 
                // EndPoint(2) khi truy vấn đến /Testpost với phương thức post hoặc put
                endpoints.MapMethods("/Testpost", new string[] { "post", "put" }, async context => {
                    await context.Response.WriteAsync("post/pust");
                });

                //  EndPoint(2) -  Middleware khi truy cập /Home với phương thức GET - nó làm Middleware cuối Pipeline
                endpoints.MapGet("/Home", async context => {

                    var getPamraUrl = context.GetRouteValue("action") ?? "value default";

                    int? count = context.Session.GetInt32("count");
                    count = (count != null) ? count + 1 : 1;
                    context.Session.SetInt32("count", count.Value);
                    await context.Response.WriteAsync($"Home page! {count}"); 
                });

                // Điểm rẽ nhánh pipeline khi URL là /Json
                app.Map("/Json", app => {
                    app.Run(async context => {
                        string Json = GetJson();

                        // Một Respone thiết lập cho biết nó trả về Json thì cần gán ContentType của Response bằng "application/json"
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(Json);
                    });
                });
            });

            //app.Use(async (context, next) => {
            //    await context.Response.WriteAsync("middleware 0 \n");
            //    await next.Invoke();
            //    await context.Response.WriteAsync("middleware quay nguoc lai \n");
            //});
            
            // call middleware theo cách truyền vào trực tiếp
            // app.UseMiddleware<CustomMiddleware>();

            // call theo cách truyền vào phương thức mở rộng 
            // app.UseMiddlewareExten();

            // dùng làm trang cảnh báo sai đường dẫn
            // khi không map được enpoint nào phù hợp thì nó sẽ nhẩy vào đây
            app.Run(async (context) => {

                // chỗ này lấy ra chuỗi kết nối trong file config
                var getConnectionStringSql = Configuration.GetSection("ConnectionStrings:SqlServerConnectionString").Value;
                var getConnectionStringOracle = Configuration.GetSection("ConnectionStrings:OracleServerConnectionString").Value;

                // cách đọc data từ mảng
                var nameStudent = Configuration.GetSection("student:0:name").Value;
                var ageStudent = Configuration.GetSection("student:0:age").Value;

                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync($"Page not found {getConnectionStringSql} + {getConnectionStringOracle} + name: {nameStudent} + age: {ageStudent}");
            });
        }
    }
}
