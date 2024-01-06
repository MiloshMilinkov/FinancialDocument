using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class TenantRepository  : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsTenantWhitelistedAsync(int tenantId)
        {
            return await _context.Tenants.AnyAsync(t => t.Id == tenantId && t.IsWhitelisted);
        }
    }
}