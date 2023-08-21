using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Identity.Client;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopPhone.Services.Implementations;
using ShopPhone.Repositories.Implementations;
using ShopPhone.Services.Mappers;
using System.Text;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddTransient<ICategoriaService, CategoriaService>();
builder.Services.AddTransient<IProductoService, ProductoService>();
builder.Services.AddTransient<IVentaService, VentaService>();
builder.Services.AddTransient<IClienteService, ClienteService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddTransient<IProductoRepository, ProductoRepository>();
builder.Services.AddTransient<IVentaRepository, VentaRepository>();
builder.Services.AddTransient<IClienteRepository, ClienteRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IFileUploader, FileUploader>();

// Add services to the container.
// Aqui mapeo el archivo de configuracion en una clase fuertemente tipada
builder.Services.Configure<AppConfig>(builder.Configuration);

builder.Services.AddDbContext<ShopPhoneContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});
/*
builder.Services.AddDefaultIdentity<IdentityUser>
    (options => options.SignIn.RequireConfirmedAccount = true).
    AddEntityFrameworkStores<ShopPhoneContext>();
*/

// Config ASP.Net Identity 
builder.Services.AddIdentity<ShopPhoneUserIdentity, IdentityRole>(policies =>
{
    policies.Password.RequireDigit = false;
    policies.Password.RequireLowercase = false;
    policies.Password.RequireUppercase = false;
    policies.Password.RequireNonAlphanumeric = false;
    policies.Password.RequiredLength = 1;
    policies.User.RequireUniqueEmail = false;
    // Politicas de bloqueo de cuentas
    policies.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    policies.Lockout.MaxFailedAccessAttempts = 90;
    policies.Lockout.AllowedForNewUsers = true;
}).AddEntityFrameworkStores<ShopPhoneContext>()
    .AddDefaultTokenProviders();

// Config log4Net
// Solo si se inyecta
//builder.Logging.ClearProviders();
//builder.Logging.AddLog4Net("log4nettest.config", true);
XmlConfigurator.Configure(new FileInfo("log4net.config"));
builder.Services.AddSingleton(LogManager.GetLogger(typeof(Program)));
builder.Logging.AddLog4Net();

// Mapppers
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CategoriaProfile>();
    config.AddProfile<ProductoProfile>();
    config.AddProfile<FacturaProfile>();
    config.AddProfile<ClienteProfile>();
});


 
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!);

    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});
 

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

// Config UserDataSeeder
/*
using (var scope = app.Services.CreateScope())
{
    await UserDataSeeder.Seed(scope.ServiceProvider);
}
*/
app.Run();
