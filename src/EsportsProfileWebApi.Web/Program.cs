using EsportsProfileWebApi.Web.Repository;
using EsportsProfileWebApi.Web.Extensions;
using EsportsProfileWebApi.Web.Mapping;
using EsportsProfileWebApi.Web.Orchestrators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthentication(builder);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerJwtBearer(); 
builder.Services.AddSingleton<IDataOrchestrator, DataOrchestrator>();
builder.Services.AddSingleton<IDataRepository, DataRepository>();
builder.Services.AddSingleton<IUserOrchestrator, UserOrchestrator>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCustomCors(builder);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();