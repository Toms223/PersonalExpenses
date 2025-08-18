using System.Globalization;
using PersonalExpenses.Extensions;
using PersonalExpenses.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddForwardedHeaders();
builder.Services.AddDataAccess(builder.Configuration);
builder.Services.AddOpenIdConnectAuth(builder.Configuration);
builder.Services.AddOutlookService();
builder.Services.AddControllersWithViews();
var culture = new CultureInfo("en-US"); // or "pt-PT", "fr-FR", etc.
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
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