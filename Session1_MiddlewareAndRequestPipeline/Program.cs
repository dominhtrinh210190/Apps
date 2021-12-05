using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Session1_MiddlewareAndRequestPipeline
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        } 

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                // đoạn này dùng làm việc với file config appsettings.json
                .ConfigureAppConfiguration((hostingcontext, config) => {
                    // optional: true nếu ko tìm thấy file .json trong app thì ko báo lỗi
                    // reloadOnChange: true, khi có sự thay đổi của file thì tự động cập nhật lại sự thay đổi đó mà không cần cập nhật lại ứng dụng
                    config.AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                { 
                    webBuilder.UseStartup<Startup>();
                })
                .UseContentRoot(Directory.GetCurrentDirectory()); 
    }
}
