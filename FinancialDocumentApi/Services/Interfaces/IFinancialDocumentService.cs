using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialDocumentApi.DTOs;

namespace FinancialDocumentApi.Services
{
    public interface IFinancialDocumentService
    {
         Task<FinancialDocumentDTO?> RetrieveAndAnonymizeDocumentAsync(int tenantId, int documentId, string productCode);
    }
}