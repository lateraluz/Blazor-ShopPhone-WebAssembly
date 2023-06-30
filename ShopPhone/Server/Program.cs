using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ShopPhone.DataAccess;
using ShopPhone.Shared.Entities;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShopPhone.Services.Implementations;
using ShopPhone.Client.Proxies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


// Add services to the container.
// Aqui mapeo el archivo de configuracion en una clase fuertemente tipada
builder.Services.Configure<AppConfig>(builder.Configuration);

builder.Services.AddDbContext<ShopphoneContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database"));

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});

// Config log4Net
// Solo si se inyecta
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net("log4nettest.config", true);

// Dependency Injection
builder.Services.AddTransient<ICategoriaService, CategoriaService>();



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

app.Run();
