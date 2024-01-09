using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace FinancialDocumentApi.Services
{
    public interface IIsClientWhiteListedService
    {
        Task<bool> IsClientIdWhitelistedAsync(int tenantId, int clientId);
    }
}