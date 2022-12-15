using Implementations;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var auth = context.Request.Headers["Authorization"];
        if (auth.IsNullOrEmpty() == false)
        {
            IEnumerable<Claim>? token;
            try
            {
                token = JwtService.DecodeToken(auth);
            }
            catch (Exception)
            {
                await context.Response.WriteAsync("ERROR");
                throw;
            }
            if (token.IsNullOrEmpty() == false)
            {
                int.TryParse(token.FirstOrDefault(claim => claim.Type == "id")?.Value, out var id);
                if (id != 0)
                    context.Items["UserId"] = id;
            }
        }
        await _next(context);
    }
}