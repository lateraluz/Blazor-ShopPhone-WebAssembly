using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;
using log4net;
using log4net.Config;
using ShopPhone.Services.Implementations;
using ShopPhone.Repositories.Implementations;
using ShopPhone.Services.Mappers;
using System.Text;
using ShopPhone.Repositories.Interfaces;
using ShopPhone.Services.Interfaces;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using ShopPhone.Server.Health;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NuGet.Protocol;
using ShopPhone.Server.Performance;

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

// Add Health checks https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks#UI-Storage-Providers
builder.Services.AddHealthChecks()
                 .AddCheck<DatabaseHealthCheck>("Database")
                 .AddCheck<ImageHealthCheck>("ImageSite");
builder.Services.AddHealthChecksUI(option =>
                                    {
                                        option.SetMinimumSecondsBetweenFailureNotifications(60);
                                        option.SetEvaluationTimeInSeconds(120); // Time in seconds between check
                                        option.MaximumHistoryEntriesPerEndpoint(60); //maximum history of checks
                                        option.SetApiMaxActiveRequests(1); //api requests concurrency
                                        option.AddHealthCheckEndpoint("My Services", "/health"); // End Point get data
                                        option.AddWebhookNotification("MyWebhook1 https://webhook.site/", uri: "https://webhook.site/456d587c-7bb1-4fe6-bbd3-9e3cd6e06826", payload: "{\"error:\":\"Error in PhoneShop\"}"); // Tested https://webhook.site works!
                                    }).AddSqlServerStorage(   
                                        connectionString: builder.Configuration.GetConnectionString("HealthDataBaseTempdb")!, 
                                        configureOptions => {
                                            
                                        },configureSqlServerOptions => {
                                            configureSqlServerOptions.EnableRetryOnFailure(10);
                                        }
                                     );

// Add Memory Cache
builder.Services.AddMemoryCache();

// Mapping AppConfig Class to read  appsettings.json
builder.Services.Configure<AppConfig>(builder.Configuration);

// Config Contect DataBase
builder.Services.AddDbContext<ShopPhoneContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});
/*
 No necesary because I dont use Entity Framework for store users
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


// Automap config
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<CategoriaProfile>();
    config.AddProfile<ProductoProfile>();
    config.AddProfile<FacturaProfile>();
    config.AddProfile<ClienteProfile>();
});

//Authentication 
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

// Avoid Denied of Services 
builder.Services.AddRateLimiter(options =>
{
    options.AddConcurrencyLimiter("concurrency", options =>
    {
        options.PermitLimit = 10;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("DoS Protection. Too many requests. Please try later again... ", cancellationToken: token);
    };
});


builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


MethodTimeLogger.Logger = LogManager.GetLogger(typeof(Program)); 

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



// Force https
app.UseHttpsRedirection();
// Health ! 
app.MapHealthChecks("health", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.UseHealthChecksUI(config =>
{
    config.UIPath = "/health-ui";
});

// Config UserDataSeeder, no used
/*
using (var scope = app.Services.CreateScope())
{
    await UserDataSeeder.Seed(scope.ServiceProvider);
}
*/
app.Run();
