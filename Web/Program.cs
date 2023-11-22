using DataAccessLayer;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container:

builder.Services.AddControllersWithViews();

// Configure the EF:

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevDatabase"));
});


var app = builder.Build();

// Configure the HTTP request pipeline:

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=PlaneeritudUritused}/{id?}");

app.Run();
