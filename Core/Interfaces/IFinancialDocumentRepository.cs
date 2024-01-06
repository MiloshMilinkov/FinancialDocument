using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IFinancialDocumentRepository
    {
        Task<FinancialDocument> GetFinancialDocument(Guid tenantId, Guid documentId);
        Task<FinancialDocument> AnonymizeFinancialDocument(FinancialDocument document, string productCode);
    }
}