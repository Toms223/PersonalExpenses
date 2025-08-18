using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using PersonalExpenses.Data;
using PersonalExpenses.Model;
using PersonalExpenses.Services;
using PersonalExpenses.Services.Interfaces;

namespace PersonalExpenses.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, IConfiguration config)
    {
        string db = config["POSTGRES_DB"] ?? throw new NullReferenceException("POSTGRES_DB");
        string user = config["POSTGRES_USER"] ?? throw new NullReferenceException("POSTGRES_USER");
        string password = config["POSTGRES_PASSWORD"] ?? throw new NullReferenceException("POSTGRES_PASSWORD");
        
        string host = config["POSTGRES_HOST"] ?? "localhost";
        string port = config["POSTGRES_PORT"] ?? "5432";

        string connectionString = $"Host={host};Port={port};Database={db};Username={user};Password={password}";

        services.AddDbContext<ExpensesDbContext>(options => options.UseNpgsql(connectionString));
        services.AddScoped<IExpensesService,  ExpensesService>();
        return services;
    }

    public static IServiceCollection AddOpenIdConnectAuth(this IServiceCollection services, IConfiguration config)
    {
        string clientId = config["MICROSOFT_CLIENT_ID"] ?? throw new NullReferenceException("MICROSOFT_CLIENT_ID");
        string clientSecret = config["MICROSOFT_CLIENT_SECRET"] ?? throw new NullReferenceException("MICROSOFT_CLIENT_SECRET");
        services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdMicrosoftOptions(clientId, clientSecret);
        return services;
    }
    
    public static IServiceCollection AddOutlookService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<ICalenderService>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext == null)
                throw new InvalidOperationException("No active HTTP context found.");
            
            var accessToken = httpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            if (string.IsNullOrEmpty(accessToken))
                throw new InvalidOperationException("User is not authenticated or no access token available.");
            
            var authProvider = new GraphAccessTokenProvider(accessToken);

            return new CalendarService(authProvider);
        });

        return services;
    }
    
    public static IServiceCollection AddForwardedHeaders(this IServiceCollection services)
    {
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | 
                                       ForwardedHeaders.XForwardedProto | 
                                       ForwardedHeaders.XForwardedHost;
            
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
            
            options.ForwardedForHeaderName = "X-Forwarded-For";
            options.ForwardedProtoHeaderName = "X-Forwarded-Proto";
            options.ForwardedHostHeaderName = "X-Forwarded-Host";
        });
        
        return services;
    }

    private static AuthenticationBuilder AddOpenIdMicrosoftOptions(this AuthenticationBuilder builder, string clientId,  string clientSecret)
    {
        builder.AddOpenIdConnect(options =>
        {
            options.Authority = "https://login.microsoftonline.com/consumers/v2.0/";
            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.ResponseType = "code";
            options.Scope.AddOICDData();
            options.Scope.AddOutlookPermissions();
            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;

            options.Events = new OpenIdConnectEvents
            {
                OnRedirectToIdentityProvider = context =>
                {
                    // Look at the forwarded headers
                    var forwardedProto = context.Request.Headers["X-Forwarded-Proto"].FirstOrDefault();
                    if (!string.IsNullOrEmpty(forwardedProto))
                    {
                        var uriBuilder = new UriBuilder(context.ProtocolMessage.RedirectUri)
                        {
                            Scheme = forwardedProto,
                            Port = forwardedProto == "https" ? 443 : 80
                        };

                        context.ProtocolMessage.RedirectUri = uriBuilder.Uri.ToString();
                    }

                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    ClaimsIdentity claimsIdentity = (ClaimsIdentity)context.Principal.Identity ?? throw new NullReferenceException("Could not find ClaimsIdentity");;
                    string email = claimsIdentity.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException("Could not find Email from ClaimsIdentity");;
                    string name = claimsIdentity.FindFirst("name")?.Value ?? throw new NullReferenceException("Could not find Name from ClaimsIdentity");;
                    var db = context.HttpContext.RequestServices.GetRequiredService<ExpensesDbContext>();
                    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
                    if (user == null)
                    {
                        user = new User
                        {
                            Email = email,
                            Name = name,
                        };
                        db.Users.Add(user);
                        await db.SaveChangesAsync();
                    }
                    claimsIdentity.AddClaim(new Claim("UserId",  user.Id.ToString()));
                }
            };
        });
        return builder;
    }

    private static void AddOICDData(this ICollection<string> scope)
    {
        scope.Add("openid");
        scope.Add("profile");
        scope.Add("email");
    }
    
    private static void AddOutlookPermissions(this ICollection<string> scope)
    {
        scope.Add("offline_access");
        scope.Add("User.Read");
        scope.Add("Calendars.ReadWrite");
    }
}