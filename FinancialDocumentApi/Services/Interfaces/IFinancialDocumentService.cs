using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using FinancialDocumentApi.DTOs;

namespace FinancialDocumentApi.Services
{
    public interface IFinancialDocumentService
    {
        Task<FinancialDocument?> RetriveDocumentAsync(int tenantId, int documentId);
        FinancialDocumentDTO? AnonymizeDocumentAsync(FinancialDocument financialDocument, string productCode);
    }
}