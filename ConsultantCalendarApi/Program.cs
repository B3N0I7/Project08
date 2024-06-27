using ConsultantApi.Repositories;
using ConsultantCalendarApi.Data;
using ConsultantCalendarApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add database connection
var connectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<ConsultantCalendarDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add HttpClient
builder.Services.AddHttpClient();

// Add dependency injection
builder.Services.AddScoped<IConsultantCalendarRepository, ConsultantCalendarRepository>();
builder.Services.AddScoped<IConsultantRepository, ConsultantRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("JwtConfiguration:Issuer").Value,
            ValidAudience = builder.Configuration.GetSection("JwtConfiguration:Audience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("JwtConfiguration:Secret").Value))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
