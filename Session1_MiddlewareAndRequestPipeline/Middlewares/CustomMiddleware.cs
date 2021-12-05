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
        // RequestDelegate là 1 con trỏ trỏ đến middleware tiếp theo
        // ta tiêm context vào hàm next để chuyển context cho middleware tiếp theo
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
