using Microsoft.OpenApi.Models;
using HRSYS.Infrastructure.Data;
using HRSYS.Application.Services;
using HRSYS.Infrastructure.Repositories;
using HRSYS.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using HRSYS.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HRSYS.Domain.Entities;
using HRSYS.Domain.Enums;
using System.Text.Json.Serialization;
using HRSYS.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

/////
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});






builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// swagger config 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HRSYS API",
        Version = "v1"
    });
});
/////////////////////Database config//////////////////////////
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

///////JWT SERVICE////////////
builder.Services.AddScoped<IJwtService, JwtService>();
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

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
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});
/////////////////////////



//////services////////////
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ILeaveRepository, LeaveRepository>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<LeaveRepository>();
builder.Services.AddScoped<LeaveService>();
 builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();









var app = builder.Build();

app.UseCors("AllowAll");

//seed hr acc
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<AppDbContext>();

    
    await db.Database.MigrateAsync();

    if (!await db.Users.AnyAsync(u => u.Role.ToString() =="HR"))
    {
        var hrUser = new User
        {
            Username = "HR",
            Password = BCrypt.Net.BCrypt.HashPassword("123"),
            Role = Role.HR,
            IsActive = true

        };

        db.Users.Add(hrUser);
        await db.SaveChangesAsync();

        Console.WriteLine(" HR account created successfully!");
    }
    else
    {
        Console.WriteLine(" HR account already exists.");
    }
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRSYS API v1");
        c.RoutePrefix = string.Empty; // يفتح Swagger مباشرة على الصفحة الرئيسية
    });
}

app.UseMiddleware<HRSYS.API.Middleware.ErrorHandlingMiddleware>();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
