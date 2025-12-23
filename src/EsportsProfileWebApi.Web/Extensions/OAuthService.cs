namespace EsportsProfileWebApi.Web.Extensions;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class OAuthService
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("key") 
            ?? throw new("FAILED GETTING KEY"));

        // Add a logger to the services
        services.AddSingleton<JwtBearerEvents>(); // optional, if you want DI injection

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.MapInboundClaims = false;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "user",
                ValidIssuer = config["Authentication:Issuer"],
                ValidAudience = config["Authentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateLifetime = true
            };

            x.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<JwtBearerEvents>>();

                    logger.LogError(context.Exception, "JWT authentication failed");

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();
        return services;
    }
}