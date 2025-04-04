using Microsoft.EntityFrameworkCore;
using L02_2022LA605_2022GO650.Models;
using L02P02_2022LA605_2022GO650.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options => 
{

    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<libreriaDbContext>(opt =>
        opt.UseSqlServer(
            builder.Configuration.GetConnectionString("libreriaDB")));
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
pattern: "{controller=Cliente}/{action=InicioVenta}/{id?}");

app.Run();
