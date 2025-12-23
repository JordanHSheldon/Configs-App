namespace EsportsProfileWebApi.Web.Extensions;

public static class CorsService
{   
    public static IServiceCollection AddCustomCors(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var config = builder.Configuration;
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder.WithOrigins("https://app.configs.cc")
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });

        return services;
    }
}
