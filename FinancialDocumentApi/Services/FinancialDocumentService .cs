using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FinancialDocumentApi.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FinancialDocumentApi.Services
{
    public class FinancialDocumentService : IFinancialDocumentService
    {
        // Dependencies injected into the service.
        private readonly IFinancialDocumentRepository _financialDocumentRepository;
        private readonly IMapper _mapper;

        // Injecting the required repository and automapper
        public FinancialDocumentService(IFinancialDocumentRepository financialDocumentRepository, IMapper mapper)
        {
            _financialDocumentRepository = financialDocumentRepository;
            _mapper = mapper;
        }
        
        // Asynchronously retrieves a financial document based on tenantId and documentId.
        // if document null, return a null and a NotFound.
        public async Task<FinancialDocument> RetriveDocumentAsync(int tenantId, int documentId)
        {
            var financialDocument = await _financialDocumentRepository.GetFinancialDocumentAsync(tenantId, documentId);
            return financialDocument;
        }

        // Anonymizes the given financial document based on the product code.
        public  FinancialDocumentDTO? AnonymizeDocumentAsync(FinancialDocument financialDocument, string productCode)
        {
            // Accept the financial document retrived based on tenantId and documentId.
            // If the document is null code does not match, return null.
            if (financialDocument == null || financialDocument.Product?.ProductCode != productCode)
                return null;

            // Parse the document template to a JObject.
            var templateJson = ParseTemplateJson(financialDocument.Product.FinancialDocumentTemplate);
                    
            // Maps the financial document to a DTO, applying anonymization as needed.
            return _mapper.Map<FinancialDocumentDTO>(ConstructDTO(financialDocument, templateJson));
        }

        // Method to construct a FinancialDocumentDTO from a FinancialDocument and template JSON.
        private FinancialDocumentDTO ConstructDTO(FinancialDocument document, JObject templateJson)
        {
            return new FinancialDocumentDTO
            {
                AccountNumber = ConstructAccountNumber(document, templateJson),
                Balance = ConstructBalance(document, templateJson),
                Transactions = ConstructTransactions(document, templateJson)
            };
        }

        // Constructs the account number field for the DTO, applying anonymization as configured in the template.
        private string?  ConstructAccountNumber(FinancialDocument document, JObject templateJson)
        {
            if (ShouldIncludeField(templateJson, "AccountNumber"))
            {
                var anonymizationType = GetAnonymizationType(templateJson, "AccountNumber");
                return ApplyAnonymization(document.AccountNumber, anonymizationType);
            }
            return null;
        }

        // Constructs the balance field for the DTO, applying anonymization as configuredin the template.
        private decimal? ConstructBalance(FinancialDocument document, JObject templateJson)
        {
            if (!ShouldIncludeField(templateJson, "Balance")){
                return null;
            } 
            var anonymizationType = GetAnonymizationType(templateJson, "Balance");
            var balance = ApplyAnonymization(document.Balance.ToString(), anonymizationType);
            return decimal.Parse(balance); // Add error handling for parsing
            
        }

        // Constructs the transactions field for the DTO, applying anonymization as configured in the template.
        private IEnumerable<TransactionDTO> ConstructTransactions(FinancialDocument document, JObject templateJson)
        {
            if (templateJson == null || !ShouldIncludeField(templateJson, "Transactions") || document.Transactions == null)
            {
                return Enumerable.Empty<TransactionDTO>();
            }   
                // Retrieves configuration for each sub-field of transactions. 
                // This goes beyond the needed and showcassed task in maybe uneeded ground.
                var transactionsConfig = GetTransactionConfig(templateJson);
                return document.Transactions.Select(transaction => AnonymizeTransactionIfNeeded(transaction, transactionsConfig)).ToList();
        }

        
        // Applies anonymization to sub-fields of a transaction as set in the tempalte configuration.
        private TransactionDTO  AnonymizeTransactionIfNeeded(Transaction transaction, JObject transactionsConfig)
        {   
            var transactionDto = _mapper.Map<TransactionDTO>(transaction);
            if (transactionsConfig == null){
                return transactionDto;
            }
                
            foreach (var field in transactionsConfig.Properties())
            {
                var fieldName = field.Name;
                var fieldConfig = field.Value as JObject;
                if (fieldConfig != null)
                {
                    var anonymizationType = fieldConfig.Value<string>("anonymize");

                    switch (fieldName)
                    {
                        // Applies anonymization to each sub-field based on its type.
                        case "TransactionId":
                            transactionDto.TransactionId = ApplyAnonymization(transactionDto.TransactionId, anonymizationType);
                            break;
                        case "Amount":
                            // Amount is a decimal, it should not be anonymized with hash/mask.
                            break;
                        case "Date":
                            // Date handling if needed, but none was directly mentioned.
                            break;
                        case "Description":
                            transactionDto.Description = ApplyAnonymization(transactionDto.Description, anonymizationType);
                            break;
                        case "Category":
                            // Category handling if needed, but none was directly mentioned.
                            break;
                    }
                }
            }
            return transactionDto;
            
        }

         // Applies the specified type of anonymization to a value.
        private string ApplyAnonymization(string value, string anonymizationType)
        {
            return anonymizationType switch
            {
                "hash" => HashValue(value),
                "mask" => "####",
                _ => value,
            };
        }

        // Retrieves the type of anonymization for a given field from the template.
        private string GetAnonymizationType(JObject template, string fieldName)
        {
            return template.SelectToken($"fields.{fieldName}.anonymize")?.ToString() ?? "";
        }

        // Determines if a field should be included in the DTO based on the template.
        public bool ShouldIncludeField(JObject template, string fieldName)
        {
            return template.SelectToken($"fields.{fieldName}.include")?.ToObject<bool>() ?? false;
        }

        // Generates a hash value for a given string.
        private string HashValue(string value)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(value));
                return Convert.ToHexString(hashBytes); 
            }
        }
        private JObject ParseTemplateJson(string template)
        {
            try
            {
                return !string.IsNullOrEmpty(template) ? JObject.Parse(template) : new JObject();
            }
            catch (JsonReaderException)
            {
                return new JObject();
            }
        }

        private JObject GetTransactionConfig(JObject templateJson)
        {
            return templateJson.SelectToken("fields.Transactions.subFields") as JObject ?? new JObject();
        }
    }
}