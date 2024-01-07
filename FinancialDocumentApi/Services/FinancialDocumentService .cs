using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FinancialDocumentApi.DTOs;

namespace FinancialDocumentApi.Services
{
    public class FinancialDocumentService : IFinancialDocumentService
    {
        private readonly IFinancialDocumentRepository _financialDocumentRepository;
        private readonly IMapper _mapper;

        public FinancialDocumentService(IFinancialDocumentRepository financialDocumentRepository, IMapper mapper)
        {
            _financialDocumentRepository = financialDocumentRepository;
            _mapper = mapper;
        }
        public async Task<FinancialDocumentDTO?> RetrieveAndAnonymizeDocumentAsync(int tenantId, int documentId, string productCode)
        {
            var financialDocument = await _financialDocumentRepository.GetFinancialDocumentAsync(tenantId, documentId);
            if (financialDocument == null)
                return null;

            var anonymizedDocument = AnonymizeDocument(financialDocument, productCode);
            return _mapper.Map<FinancialDocumentDTO>(anonymizedDocument);
        }
        private FinancialDocument AnonymizeDocument(FinancialDocument document, string productCode)
        {
                document.AccountNumber = HashValue(document.AccountNumber); // Example of anonymization
                foreach( var tran in document.Transactions){
                    tran.TransactionId = "####";
                }
                return document;
        }

        private string HashValue(string value)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
                return Convert.ToHexString(hashBytes); 
            }
        }
    }
}