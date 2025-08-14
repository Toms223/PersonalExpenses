using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using PersonalExpenses.Data;

namespace PersonalExpenses.Middleware;

public class EnsureUserExistsMiddleware
{
    private readonly RequestDelegate _next;

    public EnsureUserExistsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ExpensesDbContext db)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            int userId =  int.Parse(context.User.FindFirst("UserId")?.Value
                         ?? context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");

            if (userId == -1 || !await db.Users.AnyAsync(u => u.Id == userId))
            {
                await context.SignOutAsync();
                context.Response.Redirect("/Expenses/Home");
                return;
            }
        }

        await _next(context);
    }
}