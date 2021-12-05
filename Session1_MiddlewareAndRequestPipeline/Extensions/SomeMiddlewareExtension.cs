using Microsoft.AspNetCore.Builder;
using Session1_MiddlewareAndRequestPipeline.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1_MiddlewareAndRequestPipeline.Extensions
{
    // Extension bắt buộc phải là static class và static method
    public static class SomeMiddlewareExtension
    {
        // bắt buộc phải là phương thức static và có biến this sau lớp muốn mở rộng
        public static IApplicationBuilder UseMiddlewareExten(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
