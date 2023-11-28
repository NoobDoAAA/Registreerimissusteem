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
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=PlaneeritudUritused}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=EemaldaUritus}/{Id}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=VaataUritus}/{Id}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=LisaUritus}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=MuudaUritus}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=UrituseAndmed}/{Id}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=LisaEraisik}");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=UrituseOsalejad}/{Id}");

app.Run();
