using CurrencyConverter.Entities;
using CurrencyConverter.Repositories.Helpers;
using CurrencyConverter.Service.Contracts;
using CurrencyConverter.Services.DataServices;
using CurrencyConverter.Services.ExternalServices;
using CurrencyConverter.Services.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<CurrencyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
}, ServiceLifetime.Scoped);

AddServiceDependency(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authetication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference= new OpenApiReference
                    {
                        Id= "Bearer",
                        Type=ReferenceType.SecurityScheme
                    }
                },
                new List<string>()
            }
        });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options =>
{
    options.AllowAnyOrigin();
    options.AllowAnyMethod();
    options.AllowAnyHeader();

});

app.UseAuthorization();
app.MapControllers();
app.Run();

void AddServiceDependency(WebApplicationBuilder builder)
{
    builder.Services.AddScoped<IHttpHelper, HttpHelper>();
    builder.Services.AddScoped<ICurrencyService, CurrencyServices>();
    builder.Services.AddScoped<Repository<Currency>>();
    builder.Services.AddScoped<Repository<CurrencyExchangeRate>>();
    builder.Services.AddScoped<ICurrencyConverterService, CurrencyConverterService>();
    builder.Services.AddScoped<ICurrencyExchangeRateService, CurrencyExchangeRateService>();
    builder.Services.AddScoped<IUserService, UserService>();
    
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddNLog();
    });
}