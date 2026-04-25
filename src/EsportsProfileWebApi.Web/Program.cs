using EsportsProfileWebApi.Web.Repository;
using EsportsProfileWebApi.Web.Extensions;
using EsportsProfileWebApi.Web.Mapping;
using EsportsProfileWebApi.Web.Orchestrators;
using Microsoft.AspNetCore.HttpOverrides;
using EsportsProfileWebApi.Web.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownIPNetworks.Clear();
    options.KnownProxies.Clear();
});

builder.Services.AddJwtAuthentication(builder);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerJwtBearer(); 
builder.Services.AddSingleton<IPeripheralOrchestrator, PeripheralOrchestrator>();
builder.Services.AddSingleton<IPeripheralRepository, PeripheralRepository>();
builder.Services.AddSingleton<IProfileOrchestrator, ProfileOrchestrator>();
builder.Services.AddSingleton<IProfileRepository, ProfileRepository>();
builder.Services.AddSingleton<IUserOrchestrator, UserOrchestrator>();
builder.Services.AddSingleton<IStatsOrchestrator, StatsOrchestrator>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IStatsClient,StatsClient>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCustomCors(builder);
builder.Logging.AddConsole();
builder.Logging.AddSystemdConsole();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5173","http://127.0.0.1:5173","https://app.configs.cc")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseForwardedHeaders();
app.UseCors("AllowLocalhost");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();