using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Client> GetClientAsync(int tenantId, int documentId)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.TenantId == tenantId && c.DocumentId == documentId);
    }

    public async Task<bool> IsClientIdWhitelistedAsync(int clientId)
    {
        return await _context.Clients.AnyAsync(c => c.Id == clientId && _context.Tenants.Any(t => t.Id == c.TenantId && t.IsWhitelisted));
    }
}
}