
using LDConatroller;
using LDConatroller.Core;
using LDConatroller.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

builder.Services
   .AddSingleton<SimulatorService>()
   .AddSingleton(new LdCmd(builder.Configuration["LDPath"]))//这里可以用注册表去获取雷电路径
   .AddHostedService(services => services.GetService<SimulatorService>()!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
