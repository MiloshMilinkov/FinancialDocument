using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IFinancialDocumentRepository
    {
        Task<FinancialDocument> GetFinancialDocumentAsync(int tenantId, int documentId);
        Task<FinancialDocument> AnonymizeFinancialDocumentAsync(FinancialDocument document, string productCode);
    }
}