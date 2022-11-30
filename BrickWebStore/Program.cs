using BrickWebStore.DataAccess.DataContext;
using BrickWebStore.DataAccess.Repositories;
using BrickWebStore.DataAccess.Repositories.Abstractions;
using BrickWebStore.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddDbContext<AppDbContext>(opt 
    => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<AppDbContext>();

services.AddTransient<IEmailSender, EmailSender>();
services.AddScoped<ICategotyRepository, CategotyRepository>();
services.AddScoped<IBrickWebStoreRepository, BrickWebStoreRepository>();
services.AddScoped<IProductRepository, ProductRepository>();

services.AddHttpContextAccessor();
services.AddSession(opt =>
{
    opt.IdleTimeout = TimeSpan.FromMinutes(10);
    opt.Cookie.HttpOnly = true;
    opt.Cookie.IsEssential = true;
});
services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
