using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialDocumentApi.Services
{
    public interface ITenantService
    {
        Task<bool> IsTenantWhitelistedAsync(int tenantId);
    }
}