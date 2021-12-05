using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Session1_MiddlewareAndRequestPipeline.Middlewares
{
    /* Cấu trúc Middleware trong ASP.NET
       Một lớp(class) phù hợp là một Middleware trong ASP.NET nếu lớp đó có cấu trúc thỏa mãn những điều kiện sau: 
       Có một phương thức khởi tạo public (hàm tạo) với tham số thứ nhất kiểu RequestDelegate, nếu có tham số thứ 2 thì các tham số tiếp theo này phải Inject được từ DI của hệ thống
       Phải có tổi thiểu một trong hai phương thức có tên Invoke hoặc InvokeAsync với tham số nhận là HttpContext, những phương thức này phải trả về Task.
       (Dùng InvokeAsync nếu muốn áp dụng kỹ thuật bất đồng bộ - nên làm)
       Trong Invoke/InvokeAsync bạn viết code xử lý tác vụ của Middleware, sau đó quyết định chuyển đến Middleware tiếp theo bằng cách gọi RequestDeleage
       đã truyền đến trong hàm tạo, hoặc không chuyến đến Middlware tiếp theo thì cần đảm bảo lúc này HttpResponse trrong HttpContext đã phù hợp để trả về cho Client */

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
