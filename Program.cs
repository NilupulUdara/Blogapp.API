using System.Text;
using System.Text.Json.Serialization;
using Blogapp.API.Data;
using Blogapp.API.Mappings;
using Blogapp.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Blogapp.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Blogapp API", Version = "v1" });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

// CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// DB Contexts
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("BlogappConnectionString"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("BlogappConnectionString"))));

builder.Services.AddDbContext<BlogappAuthDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("BlogappAuthConnectionString"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("BlogappAuthConnectionString"))));

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Repositories
builder.Services.AddScoped<IPostRepository, SQLPostRepository>();
builder.Services.AddScoped<ICommentRepository, SQLCommentRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Identity
builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("BlogappAuthDb")
    .AddEntityFrameworkStores<BlogappAuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
        };
    });

var app = builder.Build();

// Development tools
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware pipeline
app.UseMiddleware<Blogapp.API.Middlewares.ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed roles/admin (optional)
// using (var scope = app.Services.CreateScope())
// {
//     var roleSeeder = new RoleSeeder();
//     await roleSeeder.SeedRolesAndAdmin(scope.ServiceProvider);
// }

app.Run();
