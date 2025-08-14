using PersonalExpenses.Extensions;
using PersonalExpenses.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddOpenIdConnectAuth(builder.Configuration);
builder.Services.AddOutlookService();
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<EnsureUserExistsMiddleware>();
app.Run();