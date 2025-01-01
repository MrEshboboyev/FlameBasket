using System.Reflection;
using Flame.BasketContext.Application;
using Flame.BasketContext.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Add Layers

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureServices(builder.Configuration);

#endregion

// Register MediatR
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


builder.Services.AddOpenApi();

var app = builder.Build();

// Apply migrations at runtime
app.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();