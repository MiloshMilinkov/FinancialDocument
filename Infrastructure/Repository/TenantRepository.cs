using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool IsTenantWhitelisted(Guid tenantId)
        {
            return _context.Tenants.Any(t => t.TenantId == tenantId && t.IsWhitelisted);
        }
    }
}