using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FinancialDocumentApi.DTOs;
using Newtonsoft.Json.Linq;

namespace FinancialDocumentApi.Services
{
    public class FinancialDocumentService : IFinancialDocumentService
    {
        private readonly IFinancialDocumentRepository _financialDocumentRepository;
        private readonly IMapper _mapper;

        //Added comments so i dont get lost in versioning(2nd logic)(REMOVE WHEN DONE!!!)
        // Injecting the require repo and automapper
        public FinancialDocumentService(IFinancialDocumentRepository financialDocumentRepository, IMapper mapper)
        {
            _financialDocumentRepository = financialDocumentRepository;
            _mapper = mapper;
        }
        public async Task<FinancialDocument?> RetriveDocumentAsync(int tenantId, int documentId)
        {
            var financialDocument = await _financialDocumentRepository.GetFinancialDocumentAsync(tenantId, documentId);
            if (financialDocument == null)
                return null;
            return financialDocument;
        }

        public  FinancialDocumentDTO? AnonymizeDocumentAsync(FinancialDocument financialDocument, string productCode)
        {
            // Accept the financial document retrived based on tenantId and documentId.
            // If the document is null code does not match, return null.
            if (financialDocument == null || financialDocument.Product?.ProductCode != productCode)
                return null;

            // Parse the document template to a JObject.
            var templateJson = financialDocument.Product?.FinancialDocumentTemplate != null 
                    ? JObject.Parse(financialDocument.Product.FinancialDocumentTemplate)
                    : new JObject();
            return _mapper.Map<FinancialDocumentDTO>(ConstructDTO(financialDocument, templateJson));
        }
        //Construct a reponse in the form of a DTO as a better practice of what the backend sends as a  response to a request.
        private FinancialDocumentDTO ConstructDTO(FinancialDocument document, JObject templateJson)
        {
            var dto = new FinancialDocumentDTO();
            dto.AccountNumber = ConstructAccountNumber(document, templateJson);
            dto.Balance = ConstructBalance(document, templateJson);
            dto.Transactions = ConstructTransactions(document, templateJson);
            return dto;
        }

        // Constructs the account number field for the DTO.
        private string?  ConstructAccountNumber(FinancialDocument document, JObject templateJson)
        {
            if (ShouldIncludeField(templateJson, "AccountNumber"))
            {
                var anonymizationType = GetAnonymizationType(templateJson, "AccountNumber");
                return ApplyAnonymization(document.AccountNumber, anonymizationType);
            }
            return null;
        }

        // Constructs the Balances field for the DTO.
        private decimal? ConstructBalance(FinancialDocument document, JObject templateJson)
        {
            if (ShouldIncludeField(templateJson, "Balance"))
            {
                var anonymizationType = GetAnonymizationType(templateJson, "Balance");
                var balance = ApplyAnonymization(document.Balance.ToString(), anonymizationType);
                return decimal.Parse(balance); // Add error handling for parsing
            }
            return null;
        }

        // Constructs the Transactions field for the DTO.
        private IEnumerable<TransactionDTO> ConstructTransactions(FinancialDocument document, JObject templateJson)
        {
            if (ShouldIncludeField(templateJson, "Transactions") && document.Transactions != null)
            {
                return document.Transactions.Select(transaction =>
                {
                    var transactionDto = _mapper.Map<TransactionDTO>(transaction);
                    var anonymizationType = GetAnonymizationType(templateJson, "TransactionId");
                    transactionDto.TransactionId = ApplyAnonymization(transaction.TransactionId, anonymizationType);
                    return transactionDto;
                }).ToList();
            }
            return Enumerable.Empty<TransactionDTO>();
        }

        // Constructs the ....(See if more fields are needed,follow the logic from above.)
        private string ApplyAnonymization(string value, string anonymizationType)
        {
            return anonymizationType switch
            {
                "hash" => HashValue(value),
                "mask" => "####",
                _ => value,
            };
        }

        //Get the anonymize type from the json we seeded (Find a better way if possible )
        private string GetAnonymizationType(JObject template, string fieldName)
        {
            return template["fields"]?[fieldName]?.Value<string>("anonymize");
        }

        //Retrive info of what cield should be incldued or not
        public bool ShouldIncludeField(JObject template, string fieldName)
        {
            var fieldTemplate = template["fields"]?[fieldName];
            return fieldTemplate != null && fieldTemplate.Value<bool>("include");
        }

        public bool ShouldAnonymizeField(JObject template, string fieldName, out string anonymizationType)
        {
            anonymizationType = null;
            var fieldTemplate = template["fields"]?[fieldName];
            if (fieldTemplate != null && fieldTemplate.Value<bool>("include"))
            {
                anonymizationType = fieldTemplate.Value<string>("anonymize");
                return !string.IsNullOrEmpty(anonymizationType);
            }

            return false;
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