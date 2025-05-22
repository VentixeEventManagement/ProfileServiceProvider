using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Domain.Handlers;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v.1.0",
        Title = "Profile Service API Documentation",
        Description = "Official doucumentation for Profile Service Provider API."
    });

    options.EnableAnnotations();
    options.ExampleFilters();

    var apiScheme = new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Description = "API key header Required. Example: X-API-KEY: your_api_key_here",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        Reference = new OpenApiReference
        {
            Id = "ApiKey",
            Type = ReferenceType.SecurityScheme,
        }
    };

    options.AddSecurityDefinition("ApiKey", apiScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiScheme, new List<string>() }
    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

builder.Services.AddCors(option => { option.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());  });
builder.Services.AddDbContext<DataContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("ProfileDatabase")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var connectionString = builder.Configuration.GetConnectionString("AzureBlobStorage");
var containerName = "images";
builder.Services.AddScoped<IAzureFileHandler>(_ => new AzureFileHandler(connectionString!, containerName));

var app = builder.Build();
app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profile service provider API ");
    c.RoutePrefix = string.Empty;
});

app.UseHsts();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();
