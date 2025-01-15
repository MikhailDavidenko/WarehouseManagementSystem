using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Business;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Data.Engine;
using WarehouseManagementSystem.Web.Infrastructure;
using FluentValidation;
using WarehouseManagementSystem.Web;
using WarehouseManagementSystem.Web.Infrastructure.Automapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining(typeof(IWebMarker));

builder.Services.AddAutoMapper(typeof(IWebMapperMarker));

builder.Services.AddContext();

builder.Services.AddRepositories();
builder.Services.AddBusinessServices();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    context.Database.Migrate();
}

app.Run();
