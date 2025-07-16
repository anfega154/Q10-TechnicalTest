using Microsoft.EntityFrameworkCore;
using Q10_TechnicalTest.Data;
using Q10_TechnicalTest.Services.Interfaces;
using Q10_TechnicalTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=students.db"));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStudentService, StudentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Students}/{action=Index}/{id?}");

app.Run();
