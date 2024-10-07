using DAL.DataAccess;
using DAL.IRepository;
using DAL.Repository;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.MapperProfile;
using Service.Options;
using Service.Services.CourseService;
using Service.Services.IdentityService;
using Service.Services.UserService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SchoolDb>(options
    =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
    );

//builder.Services.AddDbContext<ApplicationIdentityDbUser>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SchoolDb>()
    .AddDefaultTokenProviders();



// Service registration
builder.Services.AddAutoMapper(typeof(MapperProfile));
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddScoped<ISchoolRepository<User>, SchoolRepository<User>>();
builder.Services.AddScoped<ISchoolRepository<Course>, SchoolRepository<Course>>();
builder.Services.AddScoped<ICourseExtraRepository, CourseRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICourseService, CourseService>();

// Authentication configuration
var jwtSettings = new JwtSettings();
builder.Configuration.Bind(nameof(jwtSettings), jwtSettings);
builder.Services.AddSingleton(jwtSettings);

var tokenValidationParameters = new TokenValidationParameters
{
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:Secret"]))
};

builder.Services.AddSingleton(tokenValidationParameters);

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = true;
}).AddJwtBearer(x =>
{
    x.IncludeErrorDetails = true;   
    x.SaveToken = true;
    x.Audience = "Api";
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = tokenValidationParameters;
});

builder.Services.AddAuthorization();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("SchoolDb"));

builder.Services.AddControllers();
//builder.Services.AddControllersWithViews()
//    .AddNewtonsoftJson(options =>
//    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
//);
builder.Services.AddEndpointsApiExplorer();

// Swagger Configuration
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new  OpenApiInfo()
    {
        Title = "Online School",
        Version = "v1"

    });

    var security = new OpenApiSecurityRequirement()
    { };

    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http

    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }
            , Array.Empty<string>()
        }
    });
});

// var swaggerOptions = new SwaggerOptions();
// builder.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);


var app = builder.Build();

//app.MapIdentityApi<IdentityUser>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}

app.UseHsts();

app.UseRouting();

app.MapSwagger();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();