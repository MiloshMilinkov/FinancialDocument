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
    public class TenantRepository  : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Tenant> GetTenantByIdAsync(int tenantId)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.Id == tenantId);
        }
    }
}