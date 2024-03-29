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
        private readonly IIsClientWhiteListedService _isClientWhiteListedService;
        private readonly IGetClientService _getClientService;
        private readonly IMapper _mapper;
        public FinancialDocumentRetrievalController(
            IFinancialDocumentService financialDocumentService,
            IProductValidationService productValidationService,
            ICompanyService companyService,
            ITenantService tenantService,
            IIsClientWhiteListedService isClientWhiteListedService,
            IGetClientService getClientService,
            IMapper mapper)
        {
            _tenantService = tenantService;
            _companyService = companyService;
            _financialDocumentService = financialDocumentService;
            _productValidationService = productValidationService;
            _getClientService = getClientService;
            _isClientWhiteListedService = isClientWhiteListedService;
             _mapper=mapper;
        }
        
        [HttpGet("Retrieve")]
        public async Task<IActionResult> RetrieveDocument(string productCode, int tenantId, int documentId)
        {
            //Invalid Inputs
            //The task does not require direct validation of valid parameters, 
            //however, I believe that although each service validates the entity it is linked to, 
            //if the input parameter does not match the requirements, there is no need for further execution of the program flow.
            if (string.IsNullOrWhiteSpace(productCode) || tenantId <= 0 || documentId <= 0)
            {
                return BadRequest("Invalid input parameters.");
            }

            //1. Validate Product Code
            if (!await _productValidationService.IsProductSupportedAsync(productCode))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Product code not supported.");
            }

            //2. Tenant ID Whitelisting
            if (!await _tenantService.IsTenantWhitelistedAsync(tenantId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Tenant not whitelisted.");
            }
            //3. Client ID Whitelisting: Added 2 seperate service for client, only reason i did this if to follow the task given word for word.
            //I would prefer to create one service that responds to one entity and contains all the logic for that entity, for example ClientService
            //with service method  GetClientAsync(), IsClientIdWhitelistedAsync()
            //3.1 "The first service accepts TenantId and DocumentId as inputs and returns the corresponding ClientId and ClientVAT"
            //Returned as stated in the task  and I UNERSTOOD as ClientId and ClientVAT, this does go with the principal of minimal exposure
            //but i would have preferr ed to have returned the  whole Client object found.
            var client = await _getClientService.GetClientAsync(tenantId, documentId);

            //3.2 Added the response that the desired client was not found, 
            //this is not required in the task directly, but I consider it a correct check
            if (client == null){
                return NotFound("Client not found.");
            }
            else if(!await _isClientWhiteListedService.IsClientIdWhitelistedAsync(tenantId, client.Value.ClientId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Client not whitelisted.");
            }

            //4. Fetch Additional Client Information
             var company = await _companyService.GetCompanyByClientVATAsync(client.Value.ClientVAT);
            //5. Company Type Check:
            if (company == null ){
                return StatusCode(StatusCodes.Status404NotFound, "Company not found.");
            }
            else if( company.Value.CompanyType == "small")
            {
                return StatusCode(StatusCodes.Status403Forbidden, "Company type check failed.");
            }
            //6. Retrieve Financial Document.
            //If the task involved working directly with a json object and returning a string that is serialized to json in order to later manipulate it, 
            //I did not see the need for it and any advantage compared to working with class objects.
            var financialDocument = await _financialDocumentService.RetriveDocumentAsync(tenantId, documentId);
            if (financialDocument == null)
            {
                return NotFound("Financial document not found.");
            }
            //8. Financial Data Anonymization
            var anonymizedFinancialDocument = _financialDocumentService.AnonymizeDocumentAsync(financialDocument, productCode);

           
            var response = new
            {
                data = anonymizedFinancialDocument, 
                //7. Enrich Response Model
                company = new 
                {
                    registrationNumber = company.Value.RegistrationNumber,
                    companyType = company.Value.CompanyType
                }
            };
            //9. Return Response:
            return Ok(response);
        }
    }
}