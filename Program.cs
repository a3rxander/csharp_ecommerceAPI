using ecommerceAPI.src.EcommerceAPI.Application.Mapping;
using ecommerceAPI.src.EcommerceAPI.Persistence.Data;
using Microsoft.EntityFrameworkCore; 
using ecommerceAPI.src.EcommerceAPI.Domain.Repositories;
using ecommerceAPI.src.EcommerceAPI.Application.Interfaces;
using ecommerceAPI.src.EcommerceAPI.Persistence.Repositories;
using ecommerceAPI.src.EcommerceAPI.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;





var builder = WebApplication.CreateBuilder(args);
// log para probar
Console.WriteLine("DefaultConnection => " +
    builder.Configuration.GetConnectionString("DefaultConnection"));

// Add services to the container.

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers(); 

builder.Services.AddDbContext<EcommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// dependency injection
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);

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
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

//Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{ 
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Cambia la URL seg�n c�mo lo expongas
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ecommerce API V1");
        c.RoutePrefix = string.Empty; // Para que Swagger est� en la ra�z
    });
}


app.MapGet("/", () => "Hola desde Ecommerce API en Docker con Swagger");


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
