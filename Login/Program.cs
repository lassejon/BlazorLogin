using AutoMapper;
using Login.Controllers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Login.Data;
using Login.DbContext;
using Login.Hashing;
using Login.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddDbContext<LoginDbContext>((provider, opt) => opt.UseSqlServer("Name=DefaultConnection"));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseEndpoints( endpoints => endpoints.MapControllers());

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();