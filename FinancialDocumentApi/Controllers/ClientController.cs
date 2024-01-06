using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinancialDocumentApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet("{clientId}/whitelisted")]
        public async Task<IActionResult> IsClientWhitelisted(int clientId)
        {
            var isWhitelisted = await _clientRepository.IsClientIdWhitelistedAsync(clientId);
            return Ok(new { ClientId = clientId, IsWhitelisted = isWhitelisted });
        }

        [HttpGet("{tenantId}/{documentId}")]
        public async Task<IActionResult> GetClient(int tenantId, int documentId)
        {
            var client = await _clientRepository.GetClientAsync(tenantId, documentId);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }
    }
}