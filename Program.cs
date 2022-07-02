using System.Text;
using JwtAuthentication.Data;
using JwtAuthentication.Helpers;
using JwtAuthentication.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// add cors
builder.Services.AddCors();

// register db context
builder.Services.AddDbContext<UserDbContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbConnection")));
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IJwtService,JwtService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options
.WithOrigins(new[]{"http://localhost:4000"})
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
);

app.UseAuthorization();

app.MapControllers();

app.Run();
