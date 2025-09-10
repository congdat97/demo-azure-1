using Domain.Database;
using GenericRepo_Dapper.Configuration;
using GenericRepo_Dapper.Extensions;
using GenericRepo_Dapper.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Security.Claims;
using System.Text;
using Utility.Model;

var builder = WebApplication.CreateBuilder(args);

string corsName = "CorsName";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsName, policyBuilder => policyBuilder
        .WithOrigins("http://localhost", "https://localhost")
        .AllowAnyMethod()
        .AllowAnyHeader());
});

//Config
builder.Services.AddConfig(builder.Configuration);

//Connect database
builder.Services.AddSingleton<ApplicationDbContext>();

builder.Services.AddRouting(context => context.LowercaseUrls = true);
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented    = true;
});

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .WriteTo.Console()
        .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day);
});

//AutoMapper
builder.Services.AddMapping();

//Service
builder.Services.AddService(builder.Configuration);

//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", info: new OpenApiInfo { Title = "Generic Repository and Dapper API", Version = "v1" });
    option.OperationFilter<HeaderFilter>();

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token dạng: Bearer {token}"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>() // Có thể để quyền cụ thể nếu dùng Role-based auth
        }
    });
});

//JWT
var _jwtSetting = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = _jwtSetting.Issuer,
        ValidAudience = _jwtSetting.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwtSetting.SecretKey)),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name                              
    };
});


var connectDatabase = builder.Configuration.GetSection("ConnectionStrings").Get<ConnectDatabase>(); 

builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: connectDatabase.DefaultConnection,
        name: "PostgreSQL",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy
    );
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

// 1. Xử lý exception global
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseGlobalExceptionHandler();
app.UseExceptionHandler("/Error");

// 2. Routing
app.UseRouting();

// 3. JWT Authentication trước
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<RequestResponseLoggingMiddleware>();

// 4. Authorization
app.UseAuthentication();
app.UseAuthorization();

// 5. Permission check sau khi đã có User
app.UseMiddleware<PermissionMiddleware>();

// 6. Swagger
var swaggerConfig = new SwaggerConfig();
builder.Configuration.GetSection(nameof(SwaggerConfig)).Bind(swaggerConfig);
app.UseSwagger(option => { option.RouteTemplate = swaggerConfig.JsonRoute; });
app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerConfig.UIEndpoint, swaggerConfig.Description); });

// 7. CORS
app.UseCors(corsName);

// 8. Controllers
app.MapControllers();

// 9. Health checks
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

app.Run();


