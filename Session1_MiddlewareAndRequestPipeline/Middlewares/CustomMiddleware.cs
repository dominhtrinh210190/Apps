using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1_MiddlewareAndRequestPipeline.Middlewares
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync("custom middleware 1 \n"); 
            // context này để truyền qua middleware tiếp theo
            await _next(context);
            await context.Response.WriteAsync("custom middleware 2 \n");
        }
    }
}
