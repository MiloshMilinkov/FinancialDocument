using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;

namespace Infrastructure.Data
{
    public class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext context){
            if(!context.Products.Any()){
                var productsJson=File.ReadAllText("../Infrastructure/Data/DataSeed/products.json");
                var products=JsonSerializer.Deserialize<List<Product>>(productsJson);
                context.Products.AddRange(products);
            }
            if(!context.Tenants.Any()){
                var tenantsJson=File.ReadAllText("../Infrastructure/Data/DataSeed/tenants.json");
                var tenants=JsonSerializer.Deserialize<List<Tenant>>(tenantsJson);
                context.Tenants.AddRange(tenants);
            }
            if(!context.Clients.Any()){
                var clientsJson=File.ReadAllText("../Infrastructure/Data/DataSeed/clients.json");
                var clients=JsonSerializer.Deserialize<List<Client>>(clientsJson);
                context.Clients.AddRange(clients);
            }
            if(context.ChangeTracker.HasChanges()){
                await context.SaveChangesAsync();
            }
        }
    }
}