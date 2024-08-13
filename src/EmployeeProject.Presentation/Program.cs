using EmployeeProject.Application.Services;
using EmployeeProject.Core.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient and TimeWorkedService
builder.Services.AddHttpClient<TimeWorkedService>();
builder.Services.AddScoped<ITimeWorkedService, TimeWorkedService>();

// Add services to the container
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Employee}/{action=Index}/{id?}");

app.Run();
