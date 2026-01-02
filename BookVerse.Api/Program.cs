using System.Security.Claims;
using System.Text;
using Asp.Versioning;
using BookVerse.Application.Interfaces;
using BookVerse.Core.Constants;
using BookVerse.Core.Entities;
using BookVerse.Core.Models;
using BookVerse.Infrastructure.Data;
using BookVerse.Infrastructure.Repositories;
using BookVerse.Infrastructure.Services;
using BookVerse.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ====================================
// CONFIGURATION
// ====================================
builder.Configuration.AddUserSecrets<Program>();

// ====================================
// DATABASE
// ====================================

builder.Services.AddDbContext<AppDbContext>((serviceProvider,options) =>
    {
        var dateTimeProvider = serviceProvider.GetRequiredService<IDateTimeProvider>();
        var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
        
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            sqlOptions => sqlOptions.MigrationsAssembly("BookVerse.Infrastructure"));
    }
    );

// ====================================
// HTTP CONTEXT & CONTROLLERS
// ====================================


builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
}).AddNewtonsoftJson();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
// ====================================
// IDENTITY CONFIGURATION
// ====================================

builder.Services.AddIdentity<User, IdentityRole<Guid>>(opt =>
    {
        //Password requirements
        opt.Password.RequireDigit = true;
        opt.Password.RequireLowercase = true;
        opt.Password.RequireUppercase = true;
        opt.Password.RequireNonAlphanumeric = true;
        opt.Password.RequiredLength = ApplicationConstants.MinPasswordLength;

        //User requirements
        opt.User.RequireUniqueEmail = true;

        //Lockout settings
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        opt.Lockout.MaxFailedAccessAttempts = 5;
        opt.Lockout.AllowedForNewUsers = true;

        //Token providers
        opt.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;
    }).AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure password reset token lifespan
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    opt.TokenLifespan = TimeSpan.FromHours(ApplicationConstants.PasswordResetTokenExpirationHours);
});

// ====================================
// JWT AUTHENTICATION
// ====================================

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.JwtOptionsKey).Get<JwtOptions>();
    
    if (jwtOptions == null)
        throw new InvalidOperationException(
            $"JWT configuration is missing. Please configure '{JwtOptions.JwtOptionsKey}' section.");
    
    if (string.IsNullOrEmpty(jwtOptions.Secret) || jwtOptions.Secret.Length < 32)
    {
        throw new ArgumentException("JWT Secret must be at least 32 characters long.");
    }

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret)),
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.FromMinutes(5)
    };

    //Jwt events
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Authentication failed: {Message}", context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var userEmail = context.Principal?.Identity?.Name;
            logger.LogDebug("Token validated for user: {User}", userEmail);

            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("Authentication challenge: {Error} - {ErrorDescription}", 
                context.Error, context.ErrorDescription);
            return Task.CompletedTask;
        }
    };
});

// ====================================
// AUTHORIZATION POLICIES
// ====================================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(IdentityRoleConstants.Admin));
    options.AddPolicy("UserOnly", policy => policy.RequireRole(IdentityRoleConstants.User));
});

// ====================================
// SWAGGER / OPENAPI
// ====================================

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    // API Info
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BookVerse API",
        Version = "v1",
        Description = "A comprehensive bookstore API - Version 1",
        Contact = new OpenApiContact
        {
            Name = "BookVerse Support",
            Email = "support@bookverse.com"
        }
    });

    // JWT Authentication
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your JWT token."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// ====================================
// OPTIONS PATTERN CONFIGURATION
// ====================================

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.JwtOptionsKey));

builder.Services.Configure<AdminUserOptions>(
    builder.Configuration.GetSection("AdminUser"));

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection("EmailOptions"));

// ====================================
// DEPENDENCY INJECTION
// ====================================


// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IAuthorsService, AuthorsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Token Processing
builder.Services.AddScoped<IAuthTokenProcessor, AuthTokenProcessorService>();

//AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ====================================
// CORS CONFIGURATION
// ====================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });

    options.AddPolicy("ProductionPolicy", policy =>
    {
        policy.WithOrigins("https://bookverseapi.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// ====================================
// BUILD APPLICATION
// ====================================

var app = builder.Build();

// ====================================
// DATABASE SEEDING
// ====================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Starting database seeding...");

        var context = services.GetRequiredService<AppDbContext>();
        var userManager = services.GetRequiredService<UserManager<User>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var adminOptions = services.GetRequiredService<IOptions<AdminUserOptions>>();
        var dateTimeProvider = services.GetRequiredService<IDateTimeProvider>();
        await DbInitializer.SeedDataAsync(context, userManager, roleManager, adminOptions, logger,dateTimeProvider);
        logger.LogInformation("Database seeding completed successfully");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// ====================================
// MIDDLEWARE PIPELINE
// ====================================

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "BookVerse API v1");
        options.RoutePrefix = string.Empty;
    });
    app.UseCors("DevelopmentPolicy");
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseCors("ProductionPolicy");
    app.UseHsts();
}

//Security Headers middleware
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";

    await next();
});


app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();