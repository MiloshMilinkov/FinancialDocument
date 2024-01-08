using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FinancialDocumentApi.DTOs;
using FinancialDocumentApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialDocumentApi.Controllers
{
    [Route("[controller]")]
    public class FinancialDocumentRetrievalController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly ITenantService _tenantService;
        private readonly IFinancialDocumentService _financialDocumentService;
        private readonly IProductValidationService _productValidationService;
        private readonly IClientService _clientService;
        private readonly IMapper _mapper;
        public FinancialDocumentRetrievalController(
            IFinancialDocumentService financialDocumentService,
            IProductValidationService productValidationService,
            ICompanyService companyService,
            ITenantService tenantService,
            IClientService clientService,
            IMapper mapper)
        {
            _tenantService = tenantService;
            _companyService = companyService;
            _financialDocumentService = financialDocumentService;
            _productValidationService = productValidationService;
            _clientService = clientService;
             _mapper=mapper;
        }
        
        [HttpGet("Retrieve")]
        public async Task<IActionResult> RetrieveDocument(string productCode, int tenantId, int documentId)
        {
            //Invalid Inputs
            if (string.IsNullOrWhiteSpace(productCode) || tenantId <= 0 || documentId <= 0)
            {
                return BadRequest("Invalid input parameters.");
            }

            // Validate Product Code
            if (!await _productValidationService.IsProductSupportedAsync(productCode))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Product code not supported.");
            }

            //Tenant ID Whitelisting
            if (!await _tenantService.IsTenantWhitelistedAsync(tenantId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Tenant not whitelisted.");
            }
            //Client ID Whitelisting
            var client = await _clientService.GetClientAsync(tenantId, documentId);
            if (client == null || !await _clientService.IsClientIdWhitelistedAsync(client.Id))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Client not whitelisted.");
            }
            //Fetch Additional Client Information
            var company = await _companyService.GetCompanyByClientVATAsync(client.ClientVAT);
            if (company == null || company.CompanyType == "small")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Company type check failed.");
            }
            //Retrieve Financial Document, Anonymization adn DTO mapping
            var financialDocument = await _financialDocumentService.RetrieveAndAnonymizeDocumentAsync(tenantId, documentId, productCode);
            if (financialDocument == null)
            {
                return NotFound("Financial document not found.");
            }
            //Return Response
            var response = new
            {
                data = financialDocument, //will need to find a way to see if it hashed and anonymized
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