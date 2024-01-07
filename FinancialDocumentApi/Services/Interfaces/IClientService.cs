using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace FinancialDocumentApi.Services
{
    public interface IClientService
    {
        Task<Client> GetClientAsync(int tenantId, int documentId);
        Task<bool> IsClientIdWhitelistedAsync(int clientId);
    }
}