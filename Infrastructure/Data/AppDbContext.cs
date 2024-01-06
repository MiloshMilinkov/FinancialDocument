using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data 
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Tenant> Tenants { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder);
             modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
             if(Database.ProviderName=="Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties=entityType.ClrType.GetProperties().Where(p=>p.PropertyType==typeof(decimal));
                    foreach (var property in properties)
                    {
                         modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                    }
                }
            }
            //modelBuilder.Entity<Product>().HasData(ReadJson<Product>("DataSeed/products.json"));
            //modelBuilder.Entity<Tenant>().HasData(ReadJson<Tenant>("DataSeed/tenants.json"));
        }

    private List<T> ReadJson<T>(string filePath)
    {
        var jsonData = System.IO.File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<T>>(jsonData);
    }
    }
}