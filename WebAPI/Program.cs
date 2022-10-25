using API.DataAccess;
using API.DataAccess.Repository;
using API.DataAccess.Repository.IRepository;
using API.Models;
using API.Models.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

//Config Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .WriteTo.Seq("https://localhost:44384"));

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"JWT Authorization header using the Bearer scheme.
                            Enter 'Bearer' [space] and then yout token in the text input below.
                            Example: 'Bearer 1234Sabcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "0auth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });
    }
);

//Config Mapper
builder.Services.AddAutoMapper(typeof(MapperInitilizer));

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")//
    ));

//Config Repository Wrapper
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Config Identity
builder.Services.AddAuthentication();
var identity = builder.Services.AddIdentity<ApiUser, IdentityRole>(options => options.User.RequireUniqueEmail = true);
identity = new IdentityBuilder(identity.UserType, typeof(IdentityRole), builder.Services);
identity.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

//Config JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
//Key is stored as a local environmet variable through cmd commands. Use "setx KEY {val_of_key}" to set it up.
var key = Environment.GetEnvironmentVariable("KEY");
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.GetSection("Issuer").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        };
    });
builder.Services.AddScoped<IAuthMenager, AuthMenager>();

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
