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

// register db context
builder.Services.AddDbContext<UserDbContext>(options=> options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDbConnection")));
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IJwtService,JwtService>();

// adding authentication schemes
// builder.Services.AddAuthentication(options=> {
//     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// })
// .AddJwtBearer(options=>{
//     options.SaveToken = true;
//     options.RequireHttpsMetadata = false;
//     options.TokenValidationParameters = new TokenValidationParameters{
//         ValidateAudience = true,
//         ValidateIssuer = true,
//         ValidAudience = builder.Configuration["JWT:ValidAudience"],
//         ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//     };
// });
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options=>{
        options.Audience = builder.Configuration["JWT:Audience"];
        options.Authority = $"{builder.Configuration["JWT:Issuer"]}{builder.Configuration["JWT:Secret"]}";
    }
);

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
