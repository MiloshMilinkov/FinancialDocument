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

            if(!context.Companies.Any()){
                var companiesJson=File.ReadAllText("../Infrastructure/Data/DataSeed/companies.json");
                var companies=JsonSerializer.Deserialize<List<Company>>(companiesJson);
                context.Companies.AddRange(companies);
            }

            if(!context.Transactions.Any()){
                var transactionsJson=File.ReadAllText("../Infrastructure/Data/DataSeed/transactions.json");
                var transactions=JsonSerializer.Deserialize<List<Transaction>>(transactionsJson);
                context.Transactions.AddRange(transactions);
            }

            if(!context.FinancialDocuments.Any()){
                var financialDocumentsJson=File.ReadAllText("../Infrastructure/Data/DataSeed/financialDocument.json");
                var financialDocuments=JsonSerializer.Deserialize<List<FinancialDocument>>(financialDocumentsJson);
                context.FinancialDocuments.AddRange(financialDocuments);
            }

            if(!context.WhiteListedClients.Any()){
                var whiteListedClientsJson=File.ReadAllText("../Infrastructure/Data/DataSeed/whiteListedClients.json");
                var whiteListedClients=JsonSerializer.Deserialize<List<WhiteListedClient>>(whiteListedClientsJson);
                context.WhiteListedClients.AddRange(whiteListedClients);
            }

            if(context.ChangeTracker.HasChanges()){
                await context.SaveChangesAsync();
            }
        }
    }
}