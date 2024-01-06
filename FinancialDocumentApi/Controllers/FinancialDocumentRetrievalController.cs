using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using FinancialDocumentApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialDocumentApi.Controllers
{
    [Route("[controller]")]
    public class FinancialDocumentRetrievalController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IClientRepository _clientRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IFinancialDocumentRepository _financialDocumentRepository;
        public FinancialDocumentRetrievalController(
            IProductRepository productRepository,
            ITenantRepository tenantRepository,
            IClientRepository clientRepository,
            ICompanyRepository companyRepository,
            IFinancialDocumentRepository financialDocumentRepository)
        {
            _productRepository = productRepository;
            _tenantRepository = tenantRepository;
            _clientRepository = clientRepository;
            _companyRepository = companyRepository;
            _financialDocumentRepository = financialDocumentRepository;
        }
        [HttpGet("Retrieve")]
        public async Task<IActionResult> RetrieveDocument(string productCode, int tenantId, int documentId)
        {
            // Step 1: Validate Product Code
            if (!await _productRepository.IsProductSupportedAsync(productCode))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Product code not supported.");
            }

            // Step 2: Tenant ID Whitelisting
            if (!await _tenantRepository.IsTenantWhitelistedAsync(tenantId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Tenant not whitelisted.");
            }

            // Step 3: Client ID Whitelisting
            var client = await _clientRepository.GetClientAsync(tenantId, documentId);
            if (client == null || !await _clientRepository.IsClientIdWhitelistedAsync(client.Id))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Client not whitelisted.");
            }

            // Step 4: Fetch Additional Client Information
            var company = await _companyRepository.GetCompanyByClientVATAsync(client.ClientVAT);
            if (company == null || company.CompanyType == "small")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Company type check failed.");
            }

            // Step 6: Retrieve Financial Document
            var financialDocument = await _financialDocumentRepository.GetFinancialDocumentAsync(tenantId, documentId);
            if (financialDocument == null)
            {
                return NotFound("Financial document not found.");
            }

            // Step 8: Financial Data Anonymization
            var anonymizedDocument = await _financialDocumentRepository.AnonymizeFinancialDocumentAsync(financialDocument, productCode);

            var financialDocumentDTO = new FinancialDocumentDTO
            {
                AccountNumber = anonymizedDocument.AccountNumber,
                Balance = anonymizedDocument.Balance,
                Currency = anonymizedDocument.Currency,
                Transactions = anonymizedDocument.Transactions.Select(t => new TransactionDTO 
                {
                    TransactionId = t.TransactionId,
                    Amount = t.Amount,
                    Date = t.Date,
                    Description = t.Description,
                    Category = t.Category
                }),
                Tenant = new TenantDTO
                {
                    Id = anonymizedDocument.Tenant.Id,
                    IsWhitelisted = anonymizedDocument.Tenant.IsWhitelisted
                }
            };
            // Step 9: Return Response
            var response = new
            {
                data = financialDocumentDTO, // Assuming it's serialized and anonymized
                company = new 
                {
                    registrationNumber = company.RegistrationNumber,
                    companyType = company.CompanyType
                }
            };

            return Ok(response);
        }
        
    }
}