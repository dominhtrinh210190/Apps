using Microsoft.AspNetCore.Builder;
using Session1_MiddlewareAndRequestPipeline.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1_MiddlewareAndRequestPipeline.Extensions
{
    public static class SomeMiddlewareExtension
    {
        public static IApplicationBuilder UseMiddlewareExten(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
