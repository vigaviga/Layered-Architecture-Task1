using Carting;
using Carting.Carting.Application.Interfaces;
using Carting.Carting.DataAccessLayer;
using Carting.Carting.Domain.Entities;
using Carting.Carting.Services.Interfaces;
using Carting.Carting.Services.Services;
using Carting.CustomMiddleware;
using Carting.DataAccessLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var liteDbConfig = builder.Configuration.GetSection("LiteDbStoragePath").Get<LiteDbOptions>();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IRepository<Cart>>(x => new Repository<Cart>(liteDbConfig.Path));
builder.Services.AddScoped<ICartService, CartService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                new HeaderApiVersionReader("x-api-version"),
                                                new MediaTypeApiVersionReader("x-api-version"));
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddVersionedApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidAudience = "https://localhost:5001/resources",
            ValidateIssuer = true,
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach(var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseAuthentication();
app.UseMiddleware<AccessTokenLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers().RequireAuthorization();

app.Run();
