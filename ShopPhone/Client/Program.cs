using Blazored.SessionStorage;
using Blazored.Toast;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopPhone.Client;
using ShopPhone.Client.Auth;
using ShopPhone.Client.Proxies;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Register 
builder.Services.AddScoped<ProxyCategoria>();
builder.Services.AddScoped<ProxyProducto>();
builder.Services.AddScoped<ProxyVenta>();
builder.Services.AddScoped<ProxyCliente>();
builder.Services.AddScoped<ProxyUser>();


builder.Services.AddSweetAlert2();
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddBlazoredToast();
//builder.Services.AddBlazoredModal();
// Auth dependency
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();



