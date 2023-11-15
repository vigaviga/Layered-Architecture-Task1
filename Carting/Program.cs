using Carting.Carting.DataAccessLayer;
using Carting.Carting.Domain.Entities;
using Carting.Carting.Services.Interfaces;
using Carting.Carting.Services.Services;
using Carting.DataAccessLayer.Interfaces;
using Carting.DataAccessLayer.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var liteDbConfig = builder.Configuration.GetSection("LiteDbStoragePath").Get<LiteDbOptions>();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IRepository<Cart>>(x => new Repository<Cart>(liteDbConfig.Path));
builder.Services.AddScoped<ICartService, CartService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
