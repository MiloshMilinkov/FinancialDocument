using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Repository
{
    public class FinancialDocumentRepository : IFinancialDocumentRepository
    {
        public Task<FinancialDocument> AnonymizeFinancialDocumentAsync(FinancialDocument document, string productCode)
        {
            throw new NotImplementedException();
        }

        public Task<FinancialDocument> GetFinancialDocumentAsync(int tenantId, int documentId)
        {
            throw new NotImplementedException();
        }
    }
}