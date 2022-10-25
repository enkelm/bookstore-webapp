using API.DataAccess;
using API.DataAccess.Repository;
using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Config Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("https://localhost:44384"));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Config Mapper
builder.Services.AddAutoMapper(typeof(MapperInitilizer));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));

//Config Repository Wrapper
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Config Identity
builder.Services.AddAuthentication();
var identity = builder.Services.AddIdentity<ApiUser, IdentityRole>(options => options.User.RequireUniqueEmail = true);
identity = new IdentityBuilder(identity.UserType, typeof(IdentityRole), builder.Services);
identity.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
