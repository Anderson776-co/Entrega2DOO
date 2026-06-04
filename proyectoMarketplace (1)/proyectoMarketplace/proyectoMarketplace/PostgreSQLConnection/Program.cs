using APIDazma.Infrastructure.Exceptions;
using Application.Orders.UseCase;
using Application.Publications.Interfaces;
using Application.Publications.UseCase;
using Application.Users.Interfaces;
using Application.Users.UseCase;
using Domain.Ports;
using Domain.Ports.Order;
using Domain.Services.Orders;
using Domain.Services.Publications;
using Domain.Services.Users;
using Infrastructure.Contexts;
using Infrastructure.Middleware;
using Infrastructure.Repositories.Orders;
using Infrastructure.Repositories.Publications;
using Infrastructure.Repositories.Users;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddMemoryCache();

// ===== Users =====
builder.Services.AddScoped<AuthUseCase>();
builder.Services.AddScoped<LogoutUseCase>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<UpdatePersonalInformationUseCase>();
builder.Services.AddScoped<GetPersonalInfoUseCase>();
builder.Services.AddScoped<deleteUserAccountUseCase>();
builder.Services.AddScoped<CreateBusinessUseCase>();
builder.Services.AddScoped<GetBusinessInfoUseCase>();
builder.Services.AddScoped<CreateMailingAddressUseCase>();
builder.Services.AddScoped<UpdateMailingAddressUseCase>();
builder.Services.AddScoped<DeleteMailingAddressUseCase>();
builder.Services.AddScoped<GetMailingAddressUseCase>();
builder.Services.AddScoped<GenerateInvitationCodeUseCase>();
builder.Services.AddScoped<JoinBusinessUseCase>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BusinessService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MailingAddressService>();
builder.Services.AddScoped<InvitationService>();

builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMailingAddressRepository, MailingAddressRepository>();
builder.Services.AddScoped<IInvitationCodeRepository, InvitationCodeRepository>();
builder.Services.AddScoped<IjwtService, jwtService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRevokedTokenRepository, RevokedTokenRepository>();

// ===== Publications =====
builder.Services.AddScoped<CreatePublicationUseCase>();
builder.Services.AddScoped<GetPublicationUseCase>();
builder.Services.AddScoped<UpdatePublicationByIdUseCase>();
builder.Services.AddScoped<DeletePublicationByIdUseCase>();
builder.Services.AddScoped<PublicationService>();
builder.Services.AddScoped<IPublicationRepository, PublicationRepository>();
builder.Services.AddScoped<IImageService, ImageService>();

// ===== Orders =====
builder.Services.AddScoped<RegisterOrderUseCase>();
builder.Services.AddScoped<GetOrderUseCase>();
builder.Services.AddScoped<UpdateStateOrderUseCase>();
builder.Services.AddScoped<DeleteOrderByIdUseCase>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

// ===== DbContexts =====
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UsersDB")));
builder.Services.AddDbContext<PublicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PublicationsDb")));
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrdersDb")));

// ===== Authentication =====
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("Auth failed: " + context.Exception.Message);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine("Token válido para: " + context.Principal!.Identity!.Name);
            return Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            context.HandleResponse(); // evita la respuesta por defecto
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "No autenticado. Llave de acceso inválida o expirada.",
                statusCode = 401
            }));
        },
        OnForbidden = async context =>
        {
            context.Response.StatusCode = 403;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                message = "No tienes permisos para acceder a este recurso.",
                statusCode = 403
            }));
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Dazma Marketplace API", Version = "v1" });

    c.MapType<string>(() => new Microsoft.OpenApi.Models.OpenApiSchema 
    {
        Type = "string",
        Example = new Microsoft.OpenApi.Any.OpenApiString("")
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Escribe 'Bearer' seguido de un espacio y tu token.\r\n\r\n Ejemplo: \"Bearer 12345abcdef\""
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();
app.UseCors();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseMiddleware<TokenRevocationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
