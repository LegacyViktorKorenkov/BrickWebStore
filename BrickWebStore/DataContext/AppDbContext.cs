using BrickWebStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BrickWebStore.DataContext
{
    public class AppDbContext : IdentityDbContext
    {
        public DbSet<Category> Category { get; set; }
        public DbSet<BrickWebStoreModel> BrickWebStoreModel { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<ShopUser> ShopUser { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder builder)
        //{
        //    IConfigurationRoot configurationRoot = new ConfigurationBuilder()
        //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
        //        .AddJsonFile("appsettings.json")
        //        .Build();

        //    builder.UseSqlServer(configurationRoot.GetConnectionString("SqlConnection"));
        //}
    }
}
