using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.API;
using TaskManager.Domain;
using TaskManager.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddEndpoints(Assembly.Load("TaskManager.API"));

var app = builder.Build();

app.AppConfigureWebApplication();

app.Run();

// Added in order tests to have access to Program class
public partial class Program { }