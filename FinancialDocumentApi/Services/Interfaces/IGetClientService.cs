using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialDocumentApi.Services
{
    public interface IGetClientService
    {
        
        Task<(int ClientId, string ClientVAT)?> GetClientAsync(int tenantId, int documentId);
    }
}