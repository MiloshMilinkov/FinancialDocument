using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;

namespace FinancialDocumentApi.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantRepository _tenantRepository;

        public TenantService(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }
        public async Task<bool> IsTenantWhitelistedAsync(int tenantId)
        {
            return await _tenantRepository.IsTenantWhitelistedAsync(tenantId);
        }
    }
}